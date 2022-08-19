using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.Company.GetCompanyOwnedTokens
{
    public class GetCompanyOwnedTokensQueryValidator : AbstractValidator<GetCompanyOwnedTokensQuery>
    {
        public GetCompanyOwnedTokensQueryValidator()
        {
            RuleFor(t => t.CompanyId).NotEmpty();
        }
    }
}