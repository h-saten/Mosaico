using FluentValidation;
using Mosaico.Domain.ProjectManagement.Abstractions;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.CreateUpdateProjectTeamMember
{
    public class CreateUpdateProjectTeamMemberCommandValidator : AbstractValidator<CreateUpdateProjectTeamMemberCommand>
    {
        public CreateUpdateProjectTeamMemberCommandValidator(IProjectDbContext context)
        {
            RuleSet("default", () =>
            {
                RuleFor(e => e.Name).NotEmpty();
                RuleFor(e => e.Position).NotEmpty();
                RuleFor(e => e.PageId).NotEmpty();
            });
        }
    }
}
