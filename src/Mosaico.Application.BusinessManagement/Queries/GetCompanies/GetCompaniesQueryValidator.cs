using FluentValidation;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanies
{
    public class GetCompaniesQueryValidator : AbstractValidator<GetCompaniesQuery>
    {
        public GetCompaniesQueryValidator(ICurrentUserContext currentUser)
        {
            RuleSet("default", () =>
            {
                RuleFor(q => q.Skip).GreaterThanOrEqualTo(0);
                RuleFor(q => q.Take).GreaterThan(0).LessThanOrEqualTo(100);
                RuleFor(q => q.UserId).Must(guid => guid == currentUser.UserId || currentUser.IsGlobalAdmin);
            });
        }
    }
}