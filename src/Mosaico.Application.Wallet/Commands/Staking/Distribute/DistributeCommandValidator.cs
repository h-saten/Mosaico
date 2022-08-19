using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Staking.Distribute
{
    public class DistributeCommandValidator : AbstractValidator<DistributeCommand>
    {
        public DistributeCommandValidator()
        {
            RuleFor(t => t.Amount).GreaterThan(0);
            RuleFor(t => t.CompanyId).NotEmpty();
            RuleFor(t => t.StakingPairId).NotEmpty();
        }
    }
}