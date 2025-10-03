using BetApp.Application.Interfaces;
using BetApp.Domain.Entities;
using BetApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.Validators
{
    public class BetSlipValidator : IBetSlipValidator
    {
        private readonly IMarketRepository _marketRepository;

        public BetSlipValidator(IMarketRepository marketRepository)
        {
            _marketRepository = marketRepository;
        }

        public async Task ValidateAsync(BetSlip betSlip)
        {
            var marketIds = betSlip.Items.Select(i => i.MarketId).ToList();

            var markets = await _marketRepository.GetByIdsAsync(marketIds);

            foreach (var item in betSlip.Items)
            {
                var market = markets.FirstOrDefault(m => m.Id == item.MarketId);

                if (market == null || !market.IsActive)
                    throw new InvalidOperationException("Market is invalid or inactive!");

                if(market.IsTopOffer && betSlip.Items.Any(i=>i.MarketId != item.MarketId))
                    throw new InvalidOperationException("TopOffer cannot be combined with other markets");

                if(!Enum.IsDefined(typeof(BetType),item.Type))//(!market.Type.Contains(item.Type))
                    throw new InvalidOperationException("Invalid bet type for this market!");
            }
        }
    }
}
