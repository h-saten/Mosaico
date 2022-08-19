using FluentValidation;
using Mosaico.Application.Wallet.Commands.TokenManagement.MintToken;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.BurnToken
{
    public class BurnTokenCommandValidator : AbstractValidator<MintTokenCommand>
    {
        public BurnTokenCommandValidator()
        {
            RuleFor(t => t.TokenId).NotEmpty();
            RuleFor(t => t.Amount).GreaterThan(0);
        }
    }
}