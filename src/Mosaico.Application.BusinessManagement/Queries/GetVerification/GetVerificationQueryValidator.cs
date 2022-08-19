using FluentValidation;
using Mosaico.Application.BusinessManagement.Queries.GetCompanies;

namespace Mosaico.Application.BusinessManagement.Queries.GetVerification
{
    public class GetVerificationQueryValidator : AbstractValidator<GetVerificationQuery>
    {
        public GetVerificationQueryValidator()
        {
            RuleSet("default", () =>
            {
                RuleFor(d => d.CompanyId).NotEmpty();
            });
        }
    }
}