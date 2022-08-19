using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Checkout.CreateTransakTransaction
{
    public class CreateTransakTransactionCommandValidator : AbstractValidator<CreateTransakTransactionCommand>
    {
        public CreateTransakTransactionCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.TokenAmount).GreaterThan(0);
        }
    }
}