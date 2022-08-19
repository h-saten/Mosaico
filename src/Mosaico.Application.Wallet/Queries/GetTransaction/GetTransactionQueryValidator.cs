using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.GetTransaction
{
    public class GetTransactionQueryValidator : AbstractValidator<GetTransactionQuery>
    {
        public GetTransactionQueryValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}