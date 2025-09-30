using BetApp.Worker;
using BetApp.Infrastructure;
using BetApp.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BetApp.Application.Extensions;
using BetApp.Infrastructure.Extensions;
using BetApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
{
    var configuration = hostContext.Configuration;

    var connectionString = configuration.GetConnectionString("DefaultConnection");

    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));

    services.AddApplication();
    services.AddInfrastructure(configuration);
    services.AddHostedService<OutboxWorker>();
});

await builder.RunConsoleAsync();    
     

