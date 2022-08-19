using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanySocialLinks
{
    public class GetCompanySocialLinksQueryValidator : AbstractValidator<GetCompanySocialLinksQuery>
    {
        public GetCompanySocialLinksQueryValidator()
        {
            RuleSet("default", () =>
            {
                RuleFor(q => q.CompanyId).NotEmpty();
            });
        }
    }
}