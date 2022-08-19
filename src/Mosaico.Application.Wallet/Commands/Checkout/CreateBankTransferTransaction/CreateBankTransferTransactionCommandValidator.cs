using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Checkout.CreateBankTransferTransaction
{
    public class CreateBankTransferTransactionCommandValidator : AbstractValidator<CreateBankTransferTransactionCommand>
    {
        public CreateBankTransferTransactionCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.Currency).NotEmpty();
            RuleFor(t => t.TokenAmount).GreaterThan(0);
            RuleFor(t => t.UserId).NotEmpty();
            RuleFor(t => t.FiatAmount).GreaterThan(0);
        }
    }
}