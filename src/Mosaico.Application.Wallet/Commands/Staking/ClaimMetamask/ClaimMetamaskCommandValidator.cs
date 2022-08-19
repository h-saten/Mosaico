using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Staking.ClaimMetamask
{
    public class ClaimMetamaskCommandValidator : AbstractValidator<ClaimMetamaskCommand>
    {
        public ClaimMetamaskCommandValidator()
        {
            RuleFor(t => t.UserId).NotEmpty();
            RuleFor(t => t.Wallet).NotEmpty();
            RuleFor(t => t.TransactionHash).NotEmpty();
            RuleFor(t => t.StakingPairId).NotEmpty();
        }
    }
}