using BetApp.Application.Interfaces;
using BetApp.Domain.Events;
using BetApp.Infrastructure.Persistence;
using BetApp.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;

namespace BetApp.Infrastructure.Repositories
{
    public class OutboxRepository : IOutboxRepository
    {
        private readonly AppDbContext _appDbContext;

        public OutboxRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        public async Task AddAsync(OutboxMessage message)
        {
            var entity = message.ToDbEntity();

            _appDbContext.OutboxMessages.Add(entity);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<OutboxMessage>> GetUnprocessedAsync()
        {
            var entities = await _appDbContext.OutboxMessages.AsNoTracking().Where(x => x.ProcessedOn == null).ToListAsync();
            return entities.Select(e => e.ToDomain());
        }

        public async Task MarkAsProcessedAsync(Guid id, string error = null)
        {
            var entity = await _appDbContext.OutboxMessages.FindAsync(id);

            if(entity != null)
            {
                entity.ProcessedOn = DateTime.UtcNow;
                entity.Error = error;

                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}
