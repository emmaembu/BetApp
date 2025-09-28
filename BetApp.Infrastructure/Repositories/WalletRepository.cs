using AutoMapper;
using BetApp.Application.Interfaces;
using BetApp.Domain.Entities;
using BetApp.Infrastructure.Persistence;
using BetApp.Infrastructure.Persistence.DbEntities;
using BetApp.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BetApp.Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly AppDbContext _appDbContext;

        public WalletRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Wallet> GetByIdAsync(Guid id)
        {
            var wallet =  await _appDbContext.Wallets.FindAsync(id);
            return wallet?.ToDomain();
        }

        public async Task<IEnumerable<Wallet>> GetAllAsync()
        {
            var wallets = await _appDbContext.Wallets.ToListAsync();
            return wallets.Select(e => e.ToDomain()).ToList();
        }

        public async Task AddAsync(Wallet wallet)
        {
            var walletEntity = wallet.ToDbEntity();
            await _appDbContext.Wallets.AddAsync(walletEntity);
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Wallet wallet)
        {
            var walletEntity = wallet.ToDbEntity();
            _appDbContext.Wallets.Update(walletEntity);
            await _appDbContext.SaveChangesAsync();
        }
    }
}
