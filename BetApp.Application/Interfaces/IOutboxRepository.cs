using BetApp.Domain.Events;

namespace BetApp.Application.Interfaces
{
    public interface IOutboxRepository
    {
        Task AddAsync(OutboxMessage message);

        Task<IEnumerable<OutboxMessage>> GetUnprocessedAsync();

        Task MarkAsProcessedAsync(Guid id, string error = "");
    }
}
