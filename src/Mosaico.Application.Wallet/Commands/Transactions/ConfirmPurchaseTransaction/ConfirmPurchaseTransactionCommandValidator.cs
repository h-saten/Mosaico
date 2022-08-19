using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Transactions.ConfirmPurchaseTransaction
{
    public class ConfirmPurchaseTransactionCommandValidator : AbstractValidator<ConfirmPurchaseTransactionCommand>
    {
        public ConfirmPurchaseTransactionCommandValidator()
        {
            RuleFor(t => t.Currency).NotEmpty();
            RuleFor(t => t.PayedAmount).GreaterThan(0);
        }
    }
}