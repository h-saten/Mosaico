using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.WalletToken
{
    // [Cache("{{UserId}}_{{Network}}", ExpirationInMinutes = 3)]
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class WalletTokenQuery : IRequest<WalletTokenResponse>
    {
        public string UserId { get; set; }
        public Guid TokenId { get; set; }
    }
}