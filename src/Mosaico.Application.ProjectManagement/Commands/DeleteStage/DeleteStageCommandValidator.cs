using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.DeleteStage
{
    public class DeleteStageCommandValidator : AbstractValidator<DeleteStageCommand>
    {
        public DeleteStageCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.StageId).NotEmpty();
        }
    }
}