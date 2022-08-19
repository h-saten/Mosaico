using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.ProjectTeamMemberProfile
{
    public class ProjectTeamMemberProfileCommandValidator : AbstractValidator<ProjectTeamMemberProfileCommand>
    {
        public ProjectTeamMemberProfileCommandValidator()
        {
            RuleFor(x => x.FileName).NotNull().NotEmpty();
            RuleFor(x => x.Content).NotNull().NotEmpty();
        }
    }
}
