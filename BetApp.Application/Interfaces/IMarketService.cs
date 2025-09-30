using BetApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.Interfaces
{
    public interface IMarketService
    {
        Task<IEnumerable<MarketDto>> GetTopOffersAsync();

        Task<IEnumerable<MarketDto>> GetValidMarketsForMatchAsync(Guid matchId);

        Task<Guid> AddMarketToMatchAsync(MarketDto marketDto);
    }
}
