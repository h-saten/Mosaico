using Mosaico.Application.Identity.Queries.GetUser;
using Mosaico.Base;
using System;

namespace Mosaico.Application.Identity.Queries.GetUsers
{
    public class GetUsersQueryResponse
    {
        public PaginatedResult<GetUserQueryResponse> Users { get; set; }
    }
}