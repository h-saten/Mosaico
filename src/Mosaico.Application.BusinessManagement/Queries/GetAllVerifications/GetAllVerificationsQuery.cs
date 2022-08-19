using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.BusinessManagement.Queries.GetAllVerifications
{
    [Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]
    public class GetAllVerificationsQuery : IRequest<GetAllVerificationsQueryResponse>
    {
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}