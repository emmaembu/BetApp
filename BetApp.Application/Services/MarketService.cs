using AutoMapper;
using BetApp.Application.Interfaces;
using BetApp.Domain.Entities;
using BetApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.Services
{
    public class MarketService : IMarketService
    {
        private readonly IMarketRepository _marketRepository;
        private readonly IMatchRepository _matchRepository;

        public MarketService(IMarketRepository marketRepository, IMatchRepository matchRepository)
        {
            _marketRepository = marketRepository;
            _matchRepository = matchRepository;
        }

        // Add market by rules and topp offer
        public async Task<Market> AddMarketToMatchAsync(Guid matchId, int type, decimal odds, bool isTopOffer)
        {
            if (odds < 1.0m)
                throw new ArgumentException("Odds must be >= 1.0");

            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null)
                throw new Exception("Match not found");

            var market = new Market
            {
                Id = Guid.NewGuid(),
                MatchId = match.Id,
                Type = (BetType)type,
                Odds = odds,
                IsTopOffer = isTopOffer,
                IsActive = odds >= 1.0m
            };

            await _marketRepository.AddAsync(market);
            await _marketRepository.SaveChangesAsync();

            return market;
        }

        // top offers
        public async Task<IEnumerable<Market>> GetTopOffersAsync()
        {
            return (await _marketRepository.GetAllActiveAsync()).Where(m => m.IsTopOffer);
        }
          

        // get markets for match
        public async Task<IEnumerable<Market>> GetValidMarketsForMatchAsync(Guid matchId)
        {
            return (await _marketRepository.GetMarketsByMatchIdAsync(matchId))
            .Where(m => m.IsActive && m.Odds >= 1.0m);
        }
    }
}
