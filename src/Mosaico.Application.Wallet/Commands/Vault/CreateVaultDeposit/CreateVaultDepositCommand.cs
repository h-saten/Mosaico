using System;
using MediatR;

namespace Mosaico.Application.Wallet.Commands.Vault.CreateVaultDeposit
{
    //restriction is in command handler
    public class CreateVaultDepositCommand : IRequest
    {
        public Guid VaultId { get; set; }
        public Guid TokenDistributionId { get; set; }
    }
}