using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Checkout.CreateMosaicoTransaction
{
    public class CreateMosaicoTransactionCommandValidator : AbstractValidator<CreateMosaicoTransactionCommand>
    {
        public CreateMosaicoTransactionCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.PayedAmount).GreaterThan(0);
        }
    }
}