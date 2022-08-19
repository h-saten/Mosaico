using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.BurnToken
{
    [Restricted(nameof(TokenId), Authorization.Base.Constants.Permissions.Token.CanEdit)]
    public class BurnTokenCommand : IRequest
    {
        public Guid TokenId { get; set; }
        public decimal Amount { get; set; }
    }
}