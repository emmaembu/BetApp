using BetApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BetApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletsController(IWalletService walletService) 
        {
            _walletService = walletService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWallet(Guid id)
        {
            var wallet = await _walletService.GetByIdAsync(id);
            return Ok(wallet);
        }

        [HttpPost("{id}/deposit")]
        public async Task<IActionResult> Deposit(Guid id, [FromBody] decimal amount)
        {
            await _walletService.DepositAsync(id, amount);

            return Ok(new { Message = "Deposit successful" });
        }
    }
}
