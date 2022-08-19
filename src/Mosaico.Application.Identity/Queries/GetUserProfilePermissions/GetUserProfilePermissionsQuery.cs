using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Queries.GetUserProfilePermissions
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class GetUserProfilePermissionsQuery : IRequest<GetUserProfilePermissionsQueryResponse>
    {
        public string UserId { get; set; }   
    }
}