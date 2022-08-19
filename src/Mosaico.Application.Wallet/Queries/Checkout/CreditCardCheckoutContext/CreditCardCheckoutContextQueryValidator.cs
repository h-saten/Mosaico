using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.Checkout.CreditCardCheckoutContext
{
    public class CreditCardCheckoutContextQueryValidator : AbstractValidator<CreditCardCheckoutContextQuery>
    {
        public CreditCardCheckoutContextQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.UserId).NotEmpty();
        }
    }
}