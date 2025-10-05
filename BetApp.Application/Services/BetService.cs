using BetApp.Application.DTOs;
using BetApp.Application.Interfaces;
using BetApp.Application.Mappers;
using BetApp.Application.Validators;
using BetApp.Domain.Entities;
using BetApp.Domain.Enums;
using BetApp.Domain.Events;
using StackifyLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BetApp.Application.Services
{
    public class BetService :IBetService
    {
        private readonly IBetRepository _betRepository;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IMarketRepository _marketRepository;
        private readonly IWalletRepository _walletRepository;

        public BetService(IBetRepository betRepository, IOutboxRepository outboxRepository, IMarketRepository marketRepository, IWalletRepository walletRepository)
        {
            _betRepository = betRepository;
            _outboxRepository = outboxRepository;
            _marketRepository = marketRepository;
            _walletRepository = walletRepository;
        }

        public async Task<Guid> PlaceBetAsync(BetSlipRequestDto betSlipDto)
        {
            // get only wallet data
            var wallet = await _walletRepository.GetByIdAsync(betSlipDto.WalletId);
            if (wallet == null)
                throw new Exception("Wallet not found");

            // top offers etc.
            var betSlip = betSlipDto.ToDomain();
            await CheckBetSlipAsync(betSlip);
            if (wallet.Balance < betSlip.TotalStake)
                throw new Exception("Insufficient balance!");
                    
            await _betRepository.AddAsync(betSlip);

            var payload = JsonSerializer.Serialize(betSlipDto);
            //add outbox message
            var outboxMessage = new OutboxMessage
            (type: "BetPlaced",
                aggregateId: betSlip.Id,
                payload: payload
            );

            await _outboxRepository.AddAsync(outboxMessage);

            return betSlip.Id;
        }

        public async Task<IEnumerable<BetSlipRequestDto>> GetBetsByWalletAsync(Guid id)
        {
            var betSlips = await _betRepository.GetByWalletIdAsync(id);
            return betSlips.Select(e => e.ToDto()).ToList();
        }

        private async Task CheckBetSlipAsync(BetSlip betSlip)
        {         
            var marketIds = betSlip.BetItems.Select(i => i.MarketId).ToList();

            var markets = await _marketRepository.GetByIdsAsync(marketIds);

            foreach (var item in betSlip.BetItems)
            {
                var market = markets.FirstOrDefault(m => m.Id == item.MarketId);

                if (market == null || !market.IsActive)
                    throw new InvalidOperationException("Market is invalid or inactive!");

                if(market.IsTopOffer)
                {
                    item.MarkAsTopOffer();
                }

                bool hasMixedTopOfferMatches = betSlip.BetItems.GroupBy(i => i.MatchId).Any(g => g.Any(i => i.WasTopOffer) && g.Any(i => !i.WasTopOffer));
                if(hasMixedTopOfferMatches)
                    throw new InvalidOperationException("A match cannot appear both as a top offer and a regular offer in the same bet slip.");

                bool hasMultipleTopOffers = betSlip.BetItems.Count(i => i.WasTopOffer) > 1;
                if(hasMultipleTopOffers)
                    throw new InvalidOperationException("Top offer market cannot be combined with other top offer markets.");

                if (market.Type != item.Type)
                    throw new InvalidOperationException("Selection does not match market bet type");

                if(market.Odds != item.OddsAtPlacement)
                    throw new InvalidOperationException("Selection Odds do not match market odds");


            }

        }
    }
}