

using BetApp.Application.DTOs;

namespace BetApp.Application.Interfaces
{
    public interface IBetService
    {
        Task<Guid> PlaceBetAsync(BetSlipRequestDto betSlipDto);

        Task<IEnumerable<BetSlipRequestDto>> GetBetsByWalletAsync(Guid id);
    }
}
