using BetApp.Application.DTOs;
using BetApp.Application.Interfaces;
using BetApp.Application.Mappers;
using BetApp.Domain.Entities;
using BetApp.Domain.Events;
using StackifyLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BetApp.Application.Services
{
    public class BetService :IBetService
    {
        private readonly IBetRepository _betRepository;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IBetSlipValidator _betSlipValidator;
       
        public BetService(IBetRepository betRepository, IOutboxRepository outboxRepository, IBetSlipValidator betSlipValidator)
        {
            _betRepository = betRepository;
            _outboxRepository = outboxRepository;
            _betSlipValidator = betSlipValidator;
        }

        public async Task<Guid> PlaceBetAsync(BetSlipRequestDto betSlipDto)
        {

            var betSlipId = SequentialGuid.NewGuid();
            var betSlip = betSlipDto.ToDomain(betSlipId);

            await _betSlipValidator.ValidateAsync(betSlip);// insert into try catch

            await _betRepository.AddAsync(betSlip);

            var payload = JsonSerializer.Serialize(betSlip);
            //add outbox message
            var outboxMessage = new OutboxMessage
            (type: "BetPlaced",
                aggregateId: betSlip.Id,
                payload: payload
            );

            await _outboxRepository.AddAsync(outboxMessage);

            return betSlip.Id;
        }

        public async Task<IEnumerable<BetSlipRequestDto>> GetBetsByWalletAsync(Guid id)
        {
            var betSlips = await _betRepository.GetByWalletIdAsync(id);
            return betSlips.Select(e => e.ToDto()).ToList();
        }

        //private void ValidateBetSlip(BetSlip betSlip)
        //{
        //    if (betSlip.Items.Any(i => !i.Market!.IsActive || i.Market.Odds < 1.0M))
        //        throw new InvalidOperationException("Invalid market.");

        //    if (betSlip.ContainsTopOffer)
        //    {
        //        if (betSlip.Items.Count(i => i.Market!.IsTopOffer) > 1)
        //            throw new InvalidOperationException("Cannot combine multiple Top Offers");

        //        if (betSlip.Items.Any(i => !i.Market!.IsTopOffer))
        //            throw new InvalidOperationException("Cannot mix Top Offer with regular market");
        //    }
        //}
    }
}
