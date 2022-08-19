using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.SetProjectStagePurchaseLimit
{
    public class SetProjectStagePurchaseLimitCommandValidator : AbstractValidator<SetProjectStagePurchaseLimitCommand>
    {
        public SetProjectStagePurchaseLimitCommandValidator()
        {
            RuleFor(t => t.MinimumPurchase).GreaterThan(0).WithErrorCode("INVALID_MINIMUM_PURCHASE");
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.StageId).NotEmpty();
            RuleFor(t => t.PaymentMethod).Must((m) => Domain.ProjectManagement.Constants.PaymentMethods.All.Contains(m)).WithErrorCode("INVALID_PAYMENT_METHOD");
        }
    }
}