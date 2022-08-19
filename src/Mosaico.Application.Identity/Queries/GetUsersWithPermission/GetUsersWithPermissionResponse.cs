using Mosaico.Application.Identity.Queries.GetUser;
using Mosaico.Base;

namespace Mosaico.Application.Identity.Queries.GetUsersWithPermission
{
    public class GetUsersWithPermissionResponse
    {
        public PaginatedResult<GetUserQueryResponse> Users { get; set; }
    }
}