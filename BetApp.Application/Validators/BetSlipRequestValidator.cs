using FluentValidation;
using BetApp.Application.DTOs;


namespace BetApp.Application.Validators
{
    public class BetSlipRequestValidator: AbstractValidator<BetSlipRequestDto>
    {
        public BetSlipRequestValidator()
        {
            RuleFor(b => b.WalletId).NotEmpty().WithMessage("WalletId is required!");
            RuleFor(b => b.Items).NotEmpty().WithMessage("The slip must contain at least one bet!");
            RuleForEach(b => b.Items).ChildRules(items =>
            {
                items.RuleFor(i => i.MarketId).NotEmpty().WithMessage("MarketId is required!");
                items.RuleFor(i => i.OddsAtPlacement).GreaterThanOrEqualTo(1.0M).WithMessage("The odds must be >= 1");
                items.RuleFor(i => i.BetType).NotEmpty().IsInEnum().WithMessage("BetType is requred and must be in enum!");
                items.RuleFor(i => i.Stake).GreaterThan(0M).WithMessage("The stake must be > 0");

            });
        }
    }
}
