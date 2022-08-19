using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Checkout.ConfirmBankTransfer
{
    public class ConfirmBankTransferCommandValidator : AbstractValidator<ConfirmBankTransferCommand>
    {
        public ConfirmBankTransferCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.TransactionId).NotEmpty();
        }
    }
}