using FluentValidation;
using Mosaico.Application.BusinessManagement.Queries.GetCompanies;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompany
{
    public class GetCompanyQueryValidator : AbstractValidator<GetCompanyQuery>
    {
        public GetCompanyQueryValidator()
        {
            RuleSet("default", () =>
            {
                RuleFor(d => d.UniqueIdentifier).NotEmpty();
            });
        }
    }
}