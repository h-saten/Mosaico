using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.WalletTokens
{
    [Cache("{{UserId}}_{{Network}}_{{TokenTicker}}", ExpirationInMinutes = 1)]
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class WalletTokensQuery : IRequest<WalletTokensQueryResponse>
    {
        public string UserId { get; set; }
        public string Network { get; set; }
        public string TokenTicker { get; set; }
    }
}