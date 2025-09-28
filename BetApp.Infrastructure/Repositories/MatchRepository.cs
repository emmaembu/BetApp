using AutoMapper;
using BetApp.Application.Interfaces;
using BetApp.Application.Services;
using BetApp.Domain.Entities;
using BetApp.Infrastructure.Persistence;
using BetApp.Infrastructure.Persistence.DbEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetApp.Infrastructure.Persistence.Mappings;

namespace BetApp.Infrastructure.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly AppDbContext _appDbContext;

        public MatchRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Match?> GetByIdAsync(Guid id)
        {
            var match = await _appDbContext.Matches.Include(m => m.Markets).FirstOrDefaultAsync(m => m.Id == id);
            return match?.ToDomain();
        }

        public async Task<IEnumerable<Match>> GetAllAsync()
        {
            var matches =  await _appDbContext.Matches.Include(m => m.Markets).ToListAsync();
            return matches.Select(e => e.ToDomain()).ToList();
        }

        public async Task AddAsync(Match match)
        {
            var matchEntity = match.ToDbEntity();
            await _appDbContext.Matches.AddAsync(matchEntity);
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
