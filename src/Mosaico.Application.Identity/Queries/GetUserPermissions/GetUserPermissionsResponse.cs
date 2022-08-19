using System.Collections.Generic;
using Mosaico.Application.Identity.DTOs;

namespace Mosaico.Application.Identity.Queries.GetUserPermissions
{
    public class GetUserPermissionsResponse
    {
        public List<UserPermissionDTO> Permissions { get; set; }
    }
}