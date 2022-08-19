using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.DeleteProjectTeamMember
{
    public class DeleteProjectTeamMemberCommandValidator : AbstractValidator<DeleteProjectTeamMemberCommand>
    {
        public DeleteProjectTeamMemberCommandValidator()
        {
            RuleFor(c => c.Id).NotEmpty();
            RuleFor(c => c.PageId).NotEmpty();
        }
    }
}