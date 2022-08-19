using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.ExternalExchange.UpsertExternalExchange
{
    [Restricted(nameof(TokenId), Authorization.Base.Constants.Permissions.Token.CanEdit)]
    public class UpsertExternalExchangeCommand : IRequest
    {
        [JsonIgnore]
        public Guid TokenId { get; set; }
        public Guid ExternalExchangeId { get; set; }
        public DateTimeOffset ListedAt { get; set; }
    }
}