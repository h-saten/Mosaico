using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Checkout.CreateRampTransaction
{
    public class CreateRampTransactionCommandValidator : AbstractValidator<CreateRampTransactionCommand>
    {
        public CreateRampTransactionCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.Secret).NotEmpty();
            RuleFor(t => t.TokenAmount).GreaterThan(0);
        }
    }
}