using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Vesting.CreateVesting
{
    public class CreateVestingCommandValidator : AbstractValidator<CreateVestingCommand>
    {
        public CreateVestingCommandValidator()
        {
            RuleFor(t => t.Name).NotEmpty();
            RuleFor(t => t.ImmediatePay).InclusiveBetween(0, 90);
            RuleFor(t => t.TokenId).NotEmpty();
            RuleFor(t => t.TokenAmount).GreaterThan(0);
            RuleFor(t => t.AmountOfClaims).InclusiveBetween(1, 24);
            RuleFor(t => t.NumberOfDays).InclusiveBetween(1, 5475); // 5,475 - 15 years
            RuleFor(t => t.WalletAddress).NotEmpty();
        }
    }
}