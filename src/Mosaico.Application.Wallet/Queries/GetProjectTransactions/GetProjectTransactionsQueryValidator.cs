using System.Data;
using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.GetProjectTransactions
{
    public class GetProjectTransactionsQueryValidator : AbstractValidator<GetProjectTransactionsQuery>
    {
        public GetProjectTransactionsQueryValidator()
        {
            RuleFor(q => q.Skip).GreaterThanOrEqualTo(0);
            RuleFor(q => q.Take).GreaterThan(0).LessThanOrEqualTo(100);
        }
    }
}