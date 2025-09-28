using AutoMapper;
using BetApp.Application.Interfaces;
using BetApp.Application.Services;
using BetApp.Domain.Entities;
using BetApp.Infrastructure.Persistence;
using BetApp.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;

namespace BetApp.Infrastructure.Repositories
{
    public class MarketRepository : IMarketRepository
    {
        private readonly AppDbContext _appDbContext;
        public MarketRepository(AppDbContext appDbContext)
        { 
            _appDbContext = appDbContext;
        }

        public async Task<Market?> GetByIdAsync(Guid id)
        {
            var markets = await _appDbContext.Markets.Include(m => m.Match).FirstOrDefaultAsync(m => m.Id == id);
            return markets?.ToDomain();
        }

        public async Task<IEnumerable<Market>> GetMarketsByMatchIdAsync(Guid matchId)
        {
            var markets =  await _appDbContext.Markets.Where(m => m.MatchId == matchId).ToListAsync();// da dodam odd
            return markets.Select(e => e.ToDomain()).ToList();
        }

        public async Task<IEnumerable<Market>> GetAllActiveAsync()
        {
            var markets = await _appDbContext.Markets.Where(m => m.IsActive).ToListAsync();
            return markets.Select(e => e.ToDomain()).ToList();
        }

        public async Task AddAsync(Market market)
        {
            // kvota mora biti  >= 1.0
            if (market.Odds < 1.0M)
                throw new ArgumentException("Market odds must be greater or equal to 1.0");
            var marketEntity = market.ToDbEntity();
            await _appDbContext.AddAsync(marketEntity);  
        }
        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}