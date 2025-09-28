using BetApp.Domain.Entities;

namespace BetApp.Application.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet> GetByIdAsync(Guid id);

        Task<IEnumerable<Wallet>> GetAllAsync();

        Task AddAsync(Wallet wallet);

        Task SaveChangesAsync();

        Task UpdateAsync(Wallet wallet);
    }
}
