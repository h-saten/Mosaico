using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Staking.Claim
{
    public class ClaimCommandValidator: AbstractValidator<ClaimCommand>
    {
        public ClaimCommandValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
            RuleFor(t => t.UserId).NotEmpty();
        }
    }
}