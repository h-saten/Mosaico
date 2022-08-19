using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Transactions.UpdateSalesAgent
{
    public class UpdateSalesAgentCommandValidator : AbstractValidator<UpdateSalesAgentCommand>
    {
        public UpdateSalesAgentCommandValidator()
        {
            RuleFor(t => t.TransactionId).NotEmpty();
        }
    }
}