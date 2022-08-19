using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Staking.Stake
{
    public class StakeCommandValidator : AbstractValidator<StakeCommand>
    {
        public StakeCommandValidator()
        {
            RuleFor(t => t.StakingPairId).NotEmpty();
            RuleFor(t => t.Balance).GreaterThan(0);
        }
    }
}