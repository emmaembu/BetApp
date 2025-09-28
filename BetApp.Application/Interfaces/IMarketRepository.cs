using BetApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.Interfaces
{
    public interface IMarketRepository
    {
        Task<Market?> GetByIdAsync(Guid id);

        Task<IEnumerable<Market>> GetAllActiveAsync();

        Task<IEnumerable<Market>> GetMarketsByMatchIdAsync(Guid id);

        Task AddAsync(Market market);

        Task SaveChangesAsync();

    }
}
