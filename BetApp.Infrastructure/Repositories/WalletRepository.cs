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

            if (wallet is null) throw new ArgumentNullException(nameof(wallet));

            var walletEntity = await _appDbContext.Wallets
                .FirstOrDefaultAsync(w => w.Id == wallet.Id);

            if (walletEntity == null)
                throw new InvalidOperationException("Wallet not found in database.");

            walletEntity.Balance = wallet.Balance;
            walletEntity.UpdatedAt = wallet.UpdatedAt;
           
            var lastTransaction = wallet.Transactions?.LastOrDefault();
            if (lastTransaction != null)
            {
                var transactionEntity = new TransactionEntity
                {
                    WalletId = walletEntity.Id, // 
                    Amount = lastTransaction.Amount,
                    BalanceBefore = lastTransaction.BalanceBefore,
                    BalanceAfter = lastTransaction.BalanceAfter,
                    TransactionType = (int)lastTransaction.TransactionType, 
                    CreatedAt = lastTransaction.CreatedAt,
                    Description = lastTransaction.Description
                };

                _appDbContext.Transactions.Add(transactionEntity);
            }

            // save in database
            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InvalidOperationException("Concurrency conflict while updating wallet. Please retry.", ex);
            }
        }
    }
}
