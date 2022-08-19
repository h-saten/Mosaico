using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Vesting.RedeployVesting
{
    public class RedeployVestingCommandValidator : AbstractValidator<RedeployVestingCommand>
    {
        public RedeployVestingCommandValidator()
        {
            RuleFor(t => t.VestingId).NotEmpty();
        }
    }
}