using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Transactions.UpdateTransaction
{
    public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
    {
        public UpdateTransactionCommandValidator()
        {
            RuleFor(t => t.TransactionId).NotEmpty();
        }
    }
}