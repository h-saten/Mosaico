using MediatR;
using Mosaico.Application.KangaWallet.DTOs;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.KangaWallet.Queries.UserKangaWalletBalance
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    [Cache("{{UserId}}", ExpirationInMinutes = 3)]
    public class UserKangaWalletBalanceQuery : IRequest<KangaWalletBalanceResponse>
    {
        public string UserId { get; set; }
    }
}