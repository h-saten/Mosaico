using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.Checkout.KangaCheckoutContext
{
    public class KangaCheckoutContextQueryValidator : AbstractValidator<KangaCheckoutContextQuery>
    {
        public KangaCheckoutContextQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.UserId).NotEmpty();
        }
    }
}