using FluentValidation;
using Mosaico.Application.Wallet.DTO;

namespace Mosaico.Application.Wallet.Validators
{
    public class TokenDistributionValidator : AbstractValidator<TokenDistributionDTO>
    {
        public TokenDistributionValidator()
        {
            RuleFor(t => t.Name).NotEmpty().Length(3, 50);
            RuleFor(t => t.TokenAmount).GreaterThanOrEqualTo(0.01M);
        }
    }
}