using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Core.Queries.GetCounters
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class GetCountersQuery : IRequest<GetCountersQueryResponse>
    {
        public string UserId { get; set; }
    }
}