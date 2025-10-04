using BetApp.Application.DTOs;
using BetApp.Application.Interfaces;
using BetApp.Application.Mappers;
using BetApp.Domain.Entities;
using BetApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;


namespace BetApp.Worker
{
    public class OutboxWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public OutboxWorker(IServiceProvider serviceProvider, ILogger<OutboxWorker> logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OutboxWorker started.");

            //wait for database and data
            bool dbReady = false;

            while (!dbReady && !stoppingToken.IsCancellationRequested)
            {
                try
                {

                    using var testScope = _serviceProvider.CreateScope();
                    var db = testScope.ServiceProvider.GetRequiredService<AppDbContext>();

                    await db.Wallets.AnyAsync(stoppingToken);
                    dbReady = true;
                }
                catch
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }

            while(!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
                var walletService = scope.ServiceProvider.GetRequiredService<IWalletService>();

                var messages = await outboxRepository.GetUnprocessedAsync();

                foreach (var msg in messages)
                {
                    try
                    {

                        if (msg.Type == "BetPlaced")
                        {

                            var betRequest = JsonSerializer.Deserialize<BetSlipRequestDto>(msg.Payload);

                            if (betRequest == null)
                                continue;
                            var wallet = await walletService.GetByIdAsync(betRequest.WalletId);

                            var betSlip = betRequest.ToDomain();

                            await walletService.DeductForBetAsync(betRequest!.WalletId, betSlip.TotalStake, "Bet Placed");

                            _logger.LogInformation("Processed BetPlaces message {BetId}", betSlip!.Id);

                        }
                        _logger.LogInformation("Processing message {MessageId}", msg.Id);

                        await outboxRepository.MarkAsProcessedAsync(msg.Id);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error processing message {MessageId}", msg.Id);

                        await outboxRepository.MarkAsProcessedAsync(msg.Id, ex.Message);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }
        }
    }
}
