using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.Company.CompanyWalletTokens
{
    public class CompanyWalletTokensQueryValidator : AbstractValidator<CompanyWalletTokensQuery>
    {
        public CompanyWalletTokensQueryValidator()
        {
            RuleFor(d => d.CompanyId).NotEmpty();
        }
    }
}