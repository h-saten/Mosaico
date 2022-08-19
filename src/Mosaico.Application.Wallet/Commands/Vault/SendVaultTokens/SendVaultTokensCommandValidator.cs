using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Vault.SendVaultTokens
{
    public class SendVaultTokensCommandValidator : AbstractValidator<SendVaultTokensCommand>
    {
        public SendVaultTokensCommandValidator()
        {
            RuleFor(t => t.Amount).GreaterThan(0);
            RuleFor(t => t.Recipient).NotEmpty();
            RuleFor(t => t.VaultId).NotEmpty();
        }
    }
}