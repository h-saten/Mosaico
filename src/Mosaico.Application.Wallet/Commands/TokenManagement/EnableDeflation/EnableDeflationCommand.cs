using System;
using MediatR;
using Mosaico.Application.Wallet.Queries.GetToken;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;
using Mosaico.Domain.Wallet.Entities.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.EnableDeflation
{
    [Restricted(nameof(TokenId), Authorization.Base.Constants.Permissions.Token.CanEdit)]
    [CacheReset(nameof(GetTokenQuery), "{{TokenId}}")]
    public class EnableDeflationCommand : IRequest
    {
        public Guid TokenId { get; set; }
        public decimal? TransactionPercentage { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DeflationType Type { get; set; }
        public long? BuyoutDelayInDays { get; set; }
        public decimal? BuyoutPercentage { get; set; }
        public DateTimeOffset? StartsAt { get; set; }
        public bool IsEnabled { get; set; }
    }
}