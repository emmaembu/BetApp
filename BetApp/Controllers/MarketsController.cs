using BetApp.Application.DTOs;
using BetApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BetApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketsController : ControllerBase
    {
        private readonly IMarketService _marketService;

        public MarketsController(IMarketService marketService) 
        {
            _marketService = marketService;
        }

        [HttpGet("TopOffers")]
        public async Task<IActionResult> TopOffersAsync()
        {
            var topOffers = await _marketService.GetTopOffersAsync();
            return Ok(topOffers);
        }

        [HttpGet("ValidMarkets/{matchId}")]
        public async Task<IActionResult> ValidMarketsForMatchAsync(Guid matchId)
        {
            var markets = await _marketService.GetValidMarketsForMatchAsync(matchId);
            return Ok(markets);
        }

        [HttpPost("AddToMatch")]
        public async Task<IActionResult> AddMarketToMatch([FromBody] MarketDto marketDto)
        {
            await _marketService.AddMarketToMatchAsync(marketDto);
            return Ok();
        }
    }
}
