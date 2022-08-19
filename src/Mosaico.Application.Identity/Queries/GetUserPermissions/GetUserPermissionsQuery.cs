using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Queries.GetUserPermissions
{
    [Restricted(nameof(Id), Authorization.Base.Constants.DefaultRoles.Self)]
    public class GetUserPermissionsQuery : IRequest<GetUserPermissionsResponse>
    {
        public string Id { get; set; }
        public Guid? EntityId { get; set; }
    }
}