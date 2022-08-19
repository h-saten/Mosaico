using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.Checkout.BankTransferCheckoutContext
{
    public class BankTransferCheckoutContextQueryValidator : AbstractValidator<BankTransferCheckoutContextQuery>
    {
        public BankTransferCheckoutContextQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.UserId).NotEmpty();
        }
    }
}