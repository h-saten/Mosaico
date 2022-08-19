using FluentValidation;

namespace Mosaico.Application.KangaWallet.Commands.CreateTransaction
{
    public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
    {
        public CreateTransactionCommandValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.PaymentCurrency).NotEmpty();
            RuleFor(c => c.ProjectId).NotEmpty();
            RuleFor(c => c.TokenAmount).NotNull().GreaterThan(0);
            RuleFor(c => c.CurrencyAmount).NotNull().GreaterThan(0);
        }
    }
}