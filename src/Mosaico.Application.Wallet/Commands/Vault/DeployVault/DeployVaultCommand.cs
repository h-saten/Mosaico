using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Vault.DeployVault
{
    [Restricted(nameof(TokenId), Authorization.Base.Constants.Permissions.Token.CanEdit)]
    public class DeployVaultCommand : IRequest
    {
        public Guid TokenId { get; set; }
    }
}