using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanyTeamMembers
{
    public class GetCompanyTeamMembersQueryValidator : AbstractValidator<GetCompanyTeamMembersQuery>
    {
        public GetCompanyTeamMembersQueryValidator()
        {
            RuleSet("default", () =>
            {
                RuleFor(q => q.CompanyId).NotEmpty();
            });
        }
    }
}