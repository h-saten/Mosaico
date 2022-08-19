using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompaniesPublicly
{
    public class GetCompaniesPubliclyQueryValidator : AbstractValidator<GetCompaniesPubliclyQuery>
    {
        public GetCompaniesPubliclyQueryValidator()
        {
            RuleSet("default", () =>
            {
                RuleFor(q => q.Skip).GreaterThanOrEqualTo(0);
                RuleFor(q => q.Take).GreaterThan(0).LessThanOrEqualTo(100);
            });
        }
    }
}