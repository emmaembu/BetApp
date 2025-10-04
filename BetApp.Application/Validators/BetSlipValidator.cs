using BetApp.Application.DTOs;
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
        public BetSlipValidator()
        {
        }

        public async Task ValidateAsync(BetSlipRequestDto betSlipDto)
        {
            if (betSlipDto.WalletId == Guid.Empty)
                throw new ArgumentException("WalletId cannot be empty");

            if (betSlipDto.Items == null || !betSlipDto.Items.Any())
                throw new ArgumentException("BetSlip must contain at least one BetItem");

            foreach(var item in betSlipDto.Items)
            {
                if (item.Stake <= 0)
                    throw new ArgumentException("Stake must be greater than zero");

                if (!Enum.IsDefined(typeof(BetType), item.BetType))
                    throw new ArgumentException("Invalid betType");
            }
        }
    }
}
