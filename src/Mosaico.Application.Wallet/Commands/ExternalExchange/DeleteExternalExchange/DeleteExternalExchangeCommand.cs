using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.ExternalExchange.DeleteExternalExchange
{
    [Restricted(nameof(TokenId), Authorization.Base.Constants.Permissions.Token.CanEdit)]
    public class DeleteExternalExchangeCommand : IRequest
    {
        public Guid TokenId { get; set; }
        public Guid ExternalExchangeId { get; set; }
    }
}