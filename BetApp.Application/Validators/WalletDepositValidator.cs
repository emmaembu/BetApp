using BetApp.Application.DTOs;
using FluentValidation;
using FluentValidation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetApp.Application.Validators
{
    public class WalletDepositValidator : AbstractValidator<WalletDepositDto>
    {
        public WalletDepositValidator() 
        {
            RuleFor(x=>x.Id).NotEmpty().WithMessage("WalletId must not be empty!");
            RuleFor(x=>x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Transaction description is obligatory!");
        }
    }
}
