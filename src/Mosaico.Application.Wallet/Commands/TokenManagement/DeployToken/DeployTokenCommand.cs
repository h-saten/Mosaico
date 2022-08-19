using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.DeployToken
{
    [Restricted(nameof(TokenId), Authorization.Base.Constants.Permissions.Token.CanEdit)]
    public class DeployTokenCommand : IRequest<string>
    {
        public Guid TokenId { get; set; }
    }
}