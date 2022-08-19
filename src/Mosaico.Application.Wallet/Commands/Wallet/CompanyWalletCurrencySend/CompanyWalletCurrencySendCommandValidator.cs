using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Wallet.CompanyWalletCurrencySend
{
    public class CompanyWalletSendCurrencyCommandValidator : AbstractValidator<CompanyWalletSendCurrencyCommand>
    {
        public CompanyWalletSendCurrencyCommandValidator()
        {
            RuleFor(t => t.PaymentCurrencyId).NotEmpty();
            RuleFor(c => c.Amount).GreaterThan((decimal)0.1);
            RuleFor(t => t.CompanyId).NotEmpty();
            RuleFor(t => t.Address).NotEmpty();
        }
    }
}