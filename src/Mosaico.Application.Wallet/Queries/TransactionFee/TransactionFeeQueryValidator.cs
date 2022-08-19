using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.TransactionFee
{
    public class TransactionFeeQueryValidator : AbstractValidator<TransactionFeeQuery>
    {
        public TransactionFeeQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}