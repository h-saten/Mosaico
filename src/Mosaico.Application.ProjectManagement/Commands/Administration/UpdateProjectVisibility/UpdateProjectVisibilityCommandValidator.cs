using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Administration.UpdateProjectVisibility
{
    public class UpdateProjectVisibilityCommandValidator : AbstractValidator<UpdateProjectVisibilityCommand>
    {
        public UpdateProjectVisibilityCommandValidator()
        {
            RuleFor(p => p.Id).NotNull();
            RuleFor(p => p.Visibility).NotNull();
        }
    }
}