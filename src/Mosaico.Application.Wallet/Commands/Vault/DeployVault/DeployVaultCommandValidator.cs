using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Vault.DeployVault
{
    public class DeployVaultCommandValidator : AbstractValidator<DeployVaultCommand>
    {
        public DeployVaultCommandValidator()
        {
            RuleFor(t => t.TokenId).NotNull();
        }
    }
}