using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.ExternalExchange.DeleteExternalExchange
{
    public class DeleteExternalExchangeCommandValidator : AbstractValidator<DeleteExternalExchangeCommand>
    {
        public DeleteExternalExchangeCommandValidator()
        {
            RuleFor(t => t.TokenId).NotEmpty();
            RuleFor(t => t.ExternalExchangeId).NotEmpty();
        }
    }
}