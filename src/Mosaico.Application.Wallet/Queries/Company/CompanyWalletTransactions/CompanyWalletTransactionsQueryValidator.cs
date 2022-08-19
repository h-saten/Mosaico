using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.Company.CompanyWalletTransactions
{
    public class CompanyWalletTransactionsQueryValidator : AbstractValidator<CompanyWalletTransactionsQuery>
    {
        public CompanyWalletTransactionsQueryValidator()
        {
            RuleFor(q => q.CompanyId).NotEmpty();
            RuleFor(d => d.Skip).GreaterThanOrEqualTo(0);
            RuleFor(d => d.Take).GreaterThan(0);
        }
    }
}