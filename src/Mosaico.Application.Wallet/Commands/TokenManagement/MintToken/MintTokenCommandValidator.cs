using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.MintToken
{
    public class MintTokenCommandValidator : AbstractValidator<MintTokenCommand>
    {
        public MintTokenCommandValidator()
        {
            RuleFor(t => t.TokenId).NotEmpty();
            RuleFor(t => t.Amount).GreaterThan(0);
        }
    }
}