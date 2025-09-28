

using BetApp.Application.DTOs;

namespace BetApp.Application.Interfaces
{
    public interface IBetService
    {
        Task PlaceBetAsync(BetSlipDto betSlipDto);

        Task<IEnumerable<BetSlipDto>> GetBetsByWalletAsync(Guid id);
    }
}
