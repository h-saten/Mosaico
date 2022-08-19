using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Checkout.CreateMetamaskTransaction
{
    public class CreateMetamaskTransactionCommandValidator : AbstractValidator<CreateMetamaskTransactionCommand>
    {
        public CreateMetamaskTransactionCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.Currency).NotEmpty();
            RuleFor(t => t.TokenAmount).GreaterThan(0);
            RuleFor(t => t.FiatAmount).GreaterThan(0);
        }
    }
}