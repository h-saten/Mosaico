using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.Company.CompanyTokenHolders
{
    public class CompanyTokenHoldersQueryValidator : AbstractValidator<CompanyTokenHoldersQuery>
    {
        public CompanyTokenHoldersQueryValidator()
        {
            RuleFor(d => d.CompanyId).NotEmpty();
        }
    }
}