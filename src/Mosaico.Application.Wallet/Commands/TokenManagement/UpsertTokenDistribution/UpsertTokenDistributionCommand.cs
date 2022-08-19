using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Queries.GetTokenDistribution;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.UpsertTokenDistribution
{
    [Restricted(nameof(TokenId), Authorization.Base.Constants.Permissions.Token.CanEdit)]
    [CacheReset(nameof(GetTokenDistributionQuery), "{{TokenId}}")]
    public class UpsertTokenDistributionCommand : IRequest
    {
        [JsonIgnore]
        public Guid TokenId { get; set; }
        public List<TokenDistributionDTO> TokenDistributions { get; set; } = new List<TokenDistributionDTO>();
    }
}