using BetApp.Domain.Entities;

namespace BetApp.Application.Interfaces
{
    public interface IMatchRepository
    {
        Task<Match?> GetByIdAsync(Guid id);

        Task<IEnumerable<Match>> GetAllAsync();

        Task AddAsync(Match match); 

        Task SaveChangesAsync();
    }
}
