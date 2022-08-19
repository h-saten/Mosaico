using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Transactions.ConfirmDepositTransaction
{
    public class ConfirmDepositTransactionCommandValidator : AbstractValidator<ConfirmDepositTransactionCommand>
    {
        public ConfirmDepositTransactionCommandValidator()
        {
            RuleFor(t => t.Currency).NotEmpty();
            RuleFor(t => t.PayedAmount).GreaterThan(0);
            RuleFor(t => t.TransactionId).NotEmpty();
        }
    }
}