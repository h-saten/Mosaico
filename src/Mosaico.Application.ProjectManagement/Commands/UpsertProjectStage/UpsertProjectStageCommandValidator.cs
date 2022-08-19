using System;
using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertProjectStage
{
    public class UpsertProjectStageCommandValidator : AbstractValidator<UpsertProjectStageCommand>
    {
        public UpsertProjectStageCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(s => s.Name).NotNull().NotEmpty().WithErrorCode("VALIDATION_NO_STAGE_NAME");
            RuleFor(t => t.StartDate).Must(s => s > DateTimeOffset.UtcNow);
            RuleFor(t => t.EndDate).Must((c, offset) => offset > c.StartDate);
            RuleFor(s => s.MaximumPurchase).GreaterThan(stage => stage.MinimumPurchase).WithErrorCode($"VALIDATION_INVALID_MAXIMUM_PURCHASE");
            RuleFor(s => s.MinimumPurchase).GreaterThanOrEqualTo(0).WithErrorCode($"VALIDATION_INVALID_MINIMUM_PURCHASE");
        }
    }
}