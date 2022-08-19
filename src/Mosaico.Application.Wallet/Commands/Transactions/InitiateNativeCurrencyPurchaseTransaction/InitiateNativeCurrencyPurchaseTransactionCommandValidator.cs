using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Transactions.InitiateNativeCurrencyPurchaseTransaction
{
    public class InitiateNativeCurrencyPurchaseTransactionCommandValidator : AbstractValidator<InitiateNativeCurrencyPurchaseTransactionCommand>
    {
        public InitiateNativeCurrencyPurchaseTransactionCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.TokenAmount).GreaterThan(0);
            RuleFor(t => t.PaymentProcessor).Must(p => Application.Wallet.Constants.PaymentProcessors.All.Contains(p));
        }
    }
}