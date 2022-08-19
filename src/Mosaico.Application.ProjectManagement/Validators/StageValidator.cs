using System;
using FluentValidation;
using Mosaico.Application.ProjectManagement.DTOs;

namespace Mosaico.Application.ProjectManagement.Validators
{
    public class StageValidator : AbstractValidator<StageCreationDTO>
    {
        public StageValidator()
        {
            RuleFor(s => s.Name).NotNull().NotEmpty().WithErrorCode("VALIDATION_NO_STAGE_NAME");
            RuleFor(s => s.TokenPrice).GreaterThan(0).WithErrorCode($"VALIDATION_INVALID_TOKEN_PRICE");
            RuleFor(s => s.TokensSupply).GreaterThan(0).WithErrorCode($"VALIDATION_INVALID_TOKEN_SUPPLY");
            RuleFor(s => s.MaximumPurchase).GreaterThan(stage => stage.MinimumPurchase).WithErrorCode($"VALIDATION_INVALID_MAXIMUM_PURCHASE");
            RuleFor(s => s.MinimumPurchase).GreaterThanOrEqualTo(0).WithErrorCode($"VALIDATION_INVALID_MINIMUM_PURCHASE");
        }
    }
}