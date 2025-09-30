using BetApp.Application.DTOs;
using BetApp.Application.Interfaces;
using BetApp.Application.Mappers;
using BetApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository; 

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<IEnumerable<MatchDto>> GetAllAsync()
        {
            var matches = await _matchRepository.GetAllAsync();
            return matches.Select(e => e.ToDto()).ToList();
        }

        public async Task<MatchDto> GetMatchByIdAsync(Guid id)
        {
            var match = await _matchRepository.GetByIdAsync(id);
            return match!.ToDto();
        }
    }
}
