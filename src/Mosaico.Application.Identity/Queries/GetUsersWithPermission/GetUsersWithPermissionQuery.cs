using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Queries.GetUsersWithPermission
{
    [Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]
    public class GetUsersWithPermissionQuery : IRequest<GetUsersWithPermissionResponse>
    {
        public string Key { get; set;  }
    }
}