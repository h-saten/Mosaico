using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.Checkout.MosaicoWalletCheckoutContext
{
    public class MosaicoWalletCheckoutContextQueryValidator : AbstractValidator<MosaicoWalletCheckoutContextQuery>
    {
        public MosaicoWalletCheckoutContextQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.UserId).NotEmpty();
        }
    }
}