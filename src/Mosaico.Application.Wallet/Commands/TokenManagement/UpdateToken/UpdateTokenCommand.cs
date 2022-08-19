using System;
using MediatR;
using Mosaico.Application.Wallet.Queries.GetToken;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.UpdateToken
{
    [Restricted(nameof(TokenId), Authorization.Base.Constants.Permissions.Token.CanEdit)]
    [CacheReset(nameof(GetTokenQuery), "{{TokenId}}")]
    public class UpdateTokenCommand : IRequest
    {
        public Guid TokenId { get; set; }
        public string ContractAddress { get; set; }
        public string OwnerAddress { get; set; }
        public string ContractVersion { get; set; }
    }
}