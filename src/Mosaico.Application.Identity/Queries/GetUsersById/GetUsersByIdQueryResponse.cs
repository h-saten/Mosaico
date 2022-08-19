using System.Collections.Generic;
using Mosaico.Application.Identity.Queries.GetUser;

namespace Mosaico.Application.Identity.Queries.GetUsersById
{
    public class GetUsersByIdResponse
    {
        public List<GetUserQueryResponse> Users { get; set; }
    }
}