using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.Checkout.BinancePayCheckoutContext
{
    public class BinancePayCheckoutContextQueryValidator : AbstractValidator<BinancePayCheckoutContextQuery>
    {
        public BinancePayCheckoutContextQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.UserId).NotEmpty();
        }
    }
}