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
    public class BetService :IBetService
    {
        private readonly IBetRepository _betRepository;
        private readonly IWalletService _walletService;

        public BetService(IBetRepository betRepository, IWalletService walletService)
        {
            _walletService = walletService;
            _betRepository = betRepository;
        }

        public async Task PlaceBetAsync(BetSlipDto betSlipDto)
        {
            var betSlip = betSlipDto.ToDomain();

            ValidateBetSlip(betSlip);

            await _walletService.DeductForBetAsync(betSlip.WalletId, betSlip.TotalOdds, "BetSlip placement");

            await _betRepository.AddAsync(betSlip);
        }

        public async Task<IEnumerable<BetSlipDto>> GetBetsByWalletAsync(Guid id)
        {
            var betSlips = await _betRepository.GetByWalletIdAsync(id);
            return betSlips.Select(e => e.ToDto()).ToList();
        }

        private void ValidateBetSlip(BetSlip betSlip)
        {
            if (betSlip.Items.Any(i => !i.Market!.IsActive || i.Market.Odds < 1.0M))
                throw new InvalidOperationException("Invalid market.");

            if (betSlip.ContainsTopOffer)
            {
                if (betSlip.Items.Count(i => i.Market!.IsTopOffer) > 1)
                    throw new InvalidOperationException("Cannot combine multiple Top Offers");

                if (betSlip.Items.Any(i => !i.Market!.IsTopOffer))
                    throw new InvalidOperationException("Cannot mix Top Offer with regular market");
            }
        }
    }
}
