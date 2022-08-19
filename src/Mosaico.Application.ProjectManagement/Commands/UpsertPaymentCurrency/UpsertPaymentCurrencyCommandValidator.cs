using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertPaymentCurrency
{
    public class UpsertPaymentCurrencyCommandValidator : AbstractValidator<UpsertPaymentCurrencyCommand>
    {
        public UpsertPaymentCurrencyCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.PaymentCurrencyAddress);
        }
    }
}