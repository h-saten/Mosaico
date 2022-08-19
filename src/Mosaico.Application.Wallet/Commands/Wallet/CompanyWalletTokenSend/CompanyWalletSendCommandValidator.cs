using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Wallet.CompanyWalletTokenSend
{
    public class CompanyWalletSendTokenCommandValidator : AbstractValidator<CompanyWalletSendTokenCommand>
    {
        public CompanyWalletSendTokenCommandValidator()
        {
            RuleFor(t => t.TokenId).NotEmpty();
            RuleFor(c => c.Amount).GreaterThan((decimal)0.1);
            RuleFor(t => t.CompanyId).NotEmpty();
            RuleFor(t => t.Address).NotEmpty();
        }
    }
}