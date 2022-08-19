using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Checkout.PrevalidatePurchase
{
    public class PrevalidatePurchaseCommandValidator : AbstractValidator<PrevalidatePurchaseCommand>
    {
        public PrevalidatePurchaseCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.TokenAmount).GreaterThan(0);
        }
    }
}