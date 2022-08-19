using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertPaymentMethod
{
    public class UpsertPaymentMethodCommandValidator : AbstractValidator<UpsertPaymentMethodCommand>
    {
        public UpsertPaymentMethodCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.PaymentMethodKey).NotEmpty().Must(l => Domain.ProjectManagement.Constants.PaymentMethods.All.Contains(l));
        }
    }
}