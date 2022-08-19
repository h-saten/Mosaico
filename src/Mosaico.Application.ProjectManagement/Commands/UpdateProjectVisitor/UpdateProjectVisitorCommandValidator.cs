using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.UpdateProjectVisitor
{
    public class UpdateProjectVisitorCommandValidator: AbstractValidator<UpdateProjectVisitorCommand>
    {
        public UpdateProjectVisitorCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}
