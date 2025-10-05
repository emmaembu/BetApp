using AutoMapper;
using BetApp.Application.DTOs;
using BetApp.Application.Interfaces;
using BetApp.Application.Mappers;
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
        public async Task<Guid> AddMarketToMatchAsync(AddMarketRequestDto marketDto)
        {
            if (marketDto.Odds < 1.0m)
                throw new ArgumentException("Odds must be >= 1.0");

            var match = await _matchRepository.GetByIdAsync(marketDto.MatchId);
            if (match == null)
                throw new Exception("Match not found");

            var market = marketDto.ToDomain();
            await _marketRepository.AddAsync(market);
            await _marketRepository.SaveChangesAsync();

            return market.Id;
        }

        // top offers
        public async Task<IEnumerable<MarketDto>> GetTopOffersAsync()
        {
            var markets = await _marketRepository.GetAllActiveAsync();

            var topOffers = markets.Where(m => m.IsTopOffer);

            return topOffers.Select(e => e.ToDto()).ToList();
        }
          

        // get markets for match
        public async Task<IEnumerable<MarketDto>> GetValidMarketsForMatchAsync(Guid matchId)
        {
            var markets = await _marketRepository.GetMarketsByMatchIdAsync(matchId);
            var matchMarkets = markets.Where(m => m.IsActive && m.Odds >= 1.0m);
            return matchMarkets.Select(e=>e.ToDto()).ToList();  
        }

        //get is top offer by market id 
        public async Task<bool> IsTopOfferAsync(Guid marketId)
        {
            return await _marketRepository.IsTopOffer(marketId);
        }
    }
}
