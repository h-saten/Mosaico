using MediatR;
using Mosaico.Application.ProjectManagement.Permissions;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Queries.ProjectPermission
{
    // [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class ProjectPermissionQuery : IRequest<ProjectPermissions>
    {
        public string UserId { get; set; }
        public string UniqueIdentifier { get; set; }
    }
}