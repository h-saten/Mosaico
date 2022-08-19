using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Transactions.ExportTransactions
{
    public class ExportTransactionsCommandValidator : AbstractValidator<ExportTransactionsCommand>
    {
        public ExportTransactionsCommandValidator()
        {
            RuleFor(t => t.Format).NotEmpty();
            RuleFor(t => t.Format).Must(f => Constants.ExportFormats.All.Contains(f.ToUpperInvariant()));
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}