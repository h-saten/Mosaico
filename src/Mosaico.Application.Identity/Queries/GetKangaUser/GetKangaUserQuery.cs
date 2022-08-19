using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Queries.GetKangaUser
{
    [Restricted(nameof(Id), Authorization.Base.Constants.DefaultRoles.Self)]
    public class GetKangaUserQuery : IRequest<GetKangaUserQueryResponse>
    {
        public string Id { get; set; }
    }
}
