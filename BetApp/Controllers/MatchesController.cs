using BetApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BetApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchesController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMatches()
        {
            var matches = await _matchService.GetAllAsync();
            return Ok(matches);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatchByIdAsync(Guid id)
        {
            var match = await _matchService.GetMatchByIdAsync(id);
            if(match == null) return NotFound();

            return Ok(match);
        }
    }
}
