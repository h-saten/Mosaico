using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.ExternalExchange.UpsertExternalExchange
{
    public class UpsertExternalExchangeCommandValidator : AbstractValidator<UpsertExternalExchangeCommand>
    {
        public UpsertExternalExchangeCommandValidator()
        {
            RuleFor(t => t.TokenId).NotEmpty();
            RuleFor(t => t.ExternalExchangeId).NotEmpty();
        }
    }
}