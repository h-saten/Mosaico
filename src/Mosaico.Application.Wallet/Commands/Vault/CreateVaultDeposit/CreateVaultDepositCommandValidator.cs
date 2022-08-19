using FluentValidation;

namespace Mosaico.Application.Wallet.Commands.Vault.CreateVaultDeposit
{
    public class CreateVaultDepositCommandValidator : AbstractValidator<CreateVaultDepositCommand>
    {
        public CreateVaultDepositCommandValidator()
        {
            RuleFor(t => t.VaultId).NotEmpty();
            RuleFor(t => t.TokenDistributionId).NotEmpty();
        }
    }
}