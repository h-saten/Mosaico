using System;
using FluentValidation;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Application.ProjectManagement.Validators
{
    public class ProjectStageApprovedValidator : AbstractValidator<Stage>
    {
        public ProjectStageApprovedValidator()
        {
            RuleFor(s => s.Name).NotNull().NotEmpty().WithErrorCode("VALIDATION_NO_STAGE_NAME");
            RuleFor(s => s.EndDate).NotNull().Must((stage, offset) => offset.HasValue && offset.Value > stage.StartDate).WithErrorCode($"VALIDATION_INVALID_STAGE_END_DATE");
            RuleFor(s => s.StartDate).NotNull().Must((offset) => offset > DateTimeOffset.UtcNow.Date).WithErrorCode($"VALIDATION_INVALID_STAGE_START_DATE");
            RuleFor(s => s.TokenPrice).GreaterThan(0).WithErrorCode($"VALIDATION_INVALID_TOKEN_PRICE");
            RuleFor(s => s.TokensSupply).GreaterThan(0).WithErrorCode($"VALIDATION_INVALID_TOKEN_SUPPLY");
            RuleFor(s => s.MaximumPurchase).GreaterThan(stage => stage.MinimumPurchase).WithErrorCode($"VALIDATION_INVALID_MAXIMUM_PURCHASE");
            RuleFor(s => s.MinimumPurchase).GreaterThanOrEqualTo(0).WithErrorCode($"VALIDATION_INVALID_MINIMUM_PURCHASE");
        }
    }
}