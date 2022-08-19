using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Staking.StakeMetamask
{
    public class StakeMetamaskCommandValidator : AbstractValidator<StakeMetamaskCommand>
    {
        public StakeMetamaskCommandValidator()
        {
            RuleFor(t => t.Balance).GreaterThan(0);
            RuleFor(t => t.Wallet).NotEmpty();
            RuleFor(t => t.TransactionHash).NotEmpty();
            RuleFor(t => t.StakingPairId).NotEmpty();
        }
    }
}