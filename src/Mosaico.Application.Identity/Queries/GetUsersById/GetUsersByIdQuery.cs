using System.Collections.Generic;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Queries.GetUsersById
{
    [Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]
    public class GetUsersByIdQuery : IRequest<GetUsersByIdResponse>
    {
        public List<string> UsersId { get; set; }
    }
}