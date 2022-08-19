using System;
using FluentValidation;
using Mosaico.Application.ProjectManagement.Validators;

namespace Mosaico.Application.ProjectManagement.Commands.UpdateProjectStages
{
    public class UpdateProjectStagesCommandValidator : AbstractValidator<UpdateProjectStagesCommand>
    {
        public UpdateProjectStagesCommandValidator()
        {
            RuleFor(s => s.Id).NotNull();
            RuleForEach(s => s.Stages).SetValidator(new StageValidator());
        }
    }
}