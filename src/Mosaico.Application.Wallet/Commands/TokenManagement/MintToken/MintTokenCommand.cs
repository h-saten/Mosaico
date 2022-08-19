using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.MintToken
{
    [Restricted(nameof(TokenId), Authorization.Base.Constants.Permissions.Token.CanEdit)]
    public class MintTokenCommand : IRequest
    {
        public Guid TokenId { get; set; }
        public decimal Amount { get; set; }
    }
}