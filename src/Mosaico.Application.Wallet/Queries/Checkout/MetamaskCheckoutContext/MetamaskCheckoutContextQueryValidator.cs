using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.Checkout.MetamaskCheckoutContext
{
    public class MetamaskCheckoutContextQueryValidator : AbstractValidator<MetamaskCheckoutContextQuery>
    {
        public MetamaskCheckoutContextQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.UserId).NotEmpty();
        }
    }
}