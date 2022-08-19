using FluentValidation;

namespace Mosaico.Application.KangaWallet.Commands.SaveTransaction
{
    public class SaveTransactionCommandValidator : AbstractValidator<SaveTransactionCommand>
    {
        public SaveTransactionCommandValidator()
        {
            RuleFor(c => c.TransactionId).NotEmpty();
        }
    }
}