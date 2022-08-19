using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetProjectTeamMembers
{
    public class GetProjectTeamMembersQueryValidator : AbstractValidator<GetProjectTeamMembersQuery>
    {
        public GetProjectTeamMembersQueryValidator()
        {
            RuleSet("default", () =>
            {
                RuleFor(q => q.PageId).NotEmpty();
            });
        }
    }
}