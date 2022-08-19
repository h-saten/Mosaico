using MediatR;
using Mosaico.Application.KangaWallet.DTOs;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.KangaWallet.Queries.KangaUserProfile
{
    [Restricted(nameof(UserId), Constants.DefaultRoles.Self)]
    public class KangaUserProfileQuery : IRequest<KangaProfileDto>
    {
        public string UserId { get; set; }
    }
}