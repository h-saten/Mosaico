using System;
using MediatR;

namespace Mosaico.Application.Wallet.Commands.Vault.SendVaultTokens
{
    //restriction is in handler
    public class SendVaultTokensCommand : IRequest
    {
        public Guid VaultId { get; set; }
        public decimal Amount { get; set; }
        public string Recipient { get; set; }
        public Guid TokenDistributionId { get; set; }
    }
}