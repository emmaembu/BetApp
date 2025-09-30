using BetApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.Interfaces
{
    public interface IMatchService
    {
        Task<IEnumerable<MatchDto>> GetAllAsync();

        Task<MatchDto> GetMatchByIdAsync(Guid id);
    }
}
