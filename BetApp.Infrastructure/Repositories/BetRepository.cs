using BetApp.Application.Interfaces;
using BetApp.Domain.Entities;
using BetApp.Infrastructure.Persistence;
using BetApp.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Infrastructure.Repositories
{
    public class BetRepository : IBetRepository
    {
        private readonly AppDbContext _appDbContext;
        public BetRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddAsync(BetSlip betSlip)
        {
            var betSlipEntity = betSlip.ToDbEntity();

            _appDbContext.BetSlips.Add(betSlipEntity);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<BetSlip>> GetByWalletIdAsync(Guid id)
        {
            var bets =  await _appDbContext.BetSlips.Include(bs => bs.BetItems).ThenInclude(bi => bi.Market).Where(bs => bs.WalletId == id).ToListAsync();
            return bets.Select(e => e.ToDomain()).ToList();
        }
    }
}
