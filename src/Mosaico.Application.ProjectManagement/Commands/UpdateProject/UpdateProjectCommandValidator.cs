using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.UpdateProject
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            RuleSet("default", () =>
            {
                RuleFor(c => c.ProjectId).NotEmpty();
                RuleFor(c => c.Project).NotNull();
            });
        }
    }
}