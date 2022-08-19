using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Queries.GetUsers
{
    [Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]
    public class GetUsersQuery : IRequest<GetUsersQueryResponse>
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
        public string FirstName { get; set; }
        public string Email { get; set; }
    }
}