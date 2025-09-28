using BetApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.Interfaces
{
    public interface IMarketService
    {
        Task<IEnumerable<Market>> GetTopOffersAsync();

        Task<IEnumerable<Market>> GetValidMarketsForMatchAsync(Guid matchId);

        Task<Market> AddMarketToMatchAsync(Guid matchId, int type, decimal odds, bool isTopOffer);
    }
}
