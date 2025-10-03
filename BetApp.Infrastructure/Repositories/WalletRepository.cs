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
            // Pobriši eventualne null vrijednosti
            if (wallet is null) throw new ArgumentNullException(nameof(wallet));

            // Dohvati praćeni WalletEntity iz DB-a (s RowVersion)
            var walletEntity = await _appDbContext.Wallets
                .FirstOrDefaultAsync(w => w.Id == wallet.Id);

            if (walletEntity == null)
                throw new InvalidOperationException("Wallet not found in database.");

            // Provjera RowVersion (ako domain nosi rowVersion) - opcionalno:
            // Ako tvoj domain Wallet sadrži RowVersion (byte[]), možeš provjeriti ovdje:
            // if (!wallet.RowVersion.SequenceEqual(walletEntity.RowVersion)) throw new DbUpdateConcurrencyException(...);

            // Ažuriraj polja na postojećoj entiteti (ne Attach, ne Replace)
            walletEntity.Balance = wallet.Balance;
            walletEntity.UpdatedAt = wallet.UpdatedAt;

            // Ako koristiš concurrency token (RowVersion), EF će automatski uključiti u UPDATE
            // Dodaj novu transakciju (mapiraj domain Transaction -> TransactionEntity)
            var lastTransaction = wallet.Transactions?.LastOrDefault();
            if (lastTransaction != null)
            {
                var transactionEntity = new TransactionEntity
                {
                    WalletId = walletEntity.Id, // koristimo postojeci Id iz baze
                    Amount = lastTransaction.Amount,
                    BalanceBefore = lastTransaction.BalanceBefore,
                    BalanceAfter = lastTransaction.BalanceAfter,
                    TransactionType = (int)lastTransaction.TransactionType, // prilagodi ako koristiš enum
                    CreatedAt = lastTransaction.CreatedAt,
                    Description = lastTransaction.Description
                };

                _appDbContext.Transactions.Add(transactionEntity);
            }

            // Spremi u transakciji i hvataj concurrency
            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // možeš logirati i baciti specifičnu iznimku
                throw new InvalidOperationException("Concurrency conflict while updating wallet. Please retry.", ex);
            }
        }
    }
}
