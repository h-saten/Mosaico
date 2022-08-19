using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.GetTransactionStatus
{
    public class GetTransactionStatusQueryValidator : AbstractValidator<GetTransactionStatusQuery>
    {
        public GetTransactionStatusQueryValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
        }
    }
}