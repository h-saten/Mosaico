using Mosaico.Application.Identity.Queries.GetUser;
using Mosaico.Base;
using System;

namespace Mosaico.Application.Identity.Queries.GetUsersByName
{
    public class GetUsersByNameQueryResponse
    {
        public PaginatedResult<GetUserQueryResponse> Users { get; set; }
    }
}