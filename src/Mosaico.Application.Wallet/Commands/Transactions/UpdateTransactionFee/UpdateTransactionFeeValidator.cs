using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Transactions.UpdateTransactionFee
{
    public class UpdateTransactionFeeValidator : AbstractValidator<UpdateTransactionFeeCommand>
    {
        public UpdateTransactionFeeValidator()
        {
            RuleFor(t => t.TransactionId).NotEmpty();
            RuleFor(t => t.FeePercentage).GreaterThanOrEqualTo(0);
        }
    }
}