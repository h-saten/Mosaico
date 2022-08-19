using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Administration.ApproveProject
{
    public class ApproveProjectCommandValidator : AbstractValidator<ApproveProjectCommand>
    {
        public ApproveProjectCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}