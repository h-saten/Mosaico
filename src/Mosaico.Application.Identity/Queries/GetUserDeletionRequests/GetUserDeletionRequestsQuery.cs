using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Queries.GetUserDeletionRequests
{
    [Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]
    public class GetUserDeletionRequestsQuery : IRequest<GetUserDeletionRequestsQueryResponse>
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 30;
    }
}