using System;
using MediatR;
using Mosaico.Application.Wallet.Queries.GetToken;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.EnableVesting
{
    [Restricted(nameof(TokenId), Authorization.Base.Constants.Permissions.Token.CanEdit)]
    [CacheReset(nameof(GetTokenQuery), "{{TokenId}}")]
    public class EnableVestingCommand : IRequest
    {
        public Guid TokenId { get; set; }
        public bool IsEnabled { get; set; }
    }
}