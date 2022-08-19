using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Crowdsale.DeployStage
{
    public class DeployStageCommandValidator : AbstractValidator<DeployStageCommand>
    {
        public DeployStageCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.StageId).NotEmpty();
        }
    }
}