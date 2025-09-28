using BetApp.Application.DTOs;
using BetApp.Application.Interfaces;
using BetApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BetApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BetsController : ControllerBase
    {
        private readonly IBetService _betService;
        public BetsController(IBetService betService)
        {
            _betService = betService;   
        }

        [HttpPost]
        public async Task<IActionResult> PlaceBet([FromBody] BetSlipDto betSlipDto)
        {
            try
            {
                await _betService.PlaceBetAsync(betSlipDto);
                return Ok(new { Message = "Bet placed successfully", BetSlipId = betSlipDto.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpGet("{walletId}")]
        public async Task<IActionResult> GetBets(Guid walletId)
        {
            var bets = await _betService.GetBetsByWalletAsync(walletId);
            return Ok(bets);
        }
    }
}
