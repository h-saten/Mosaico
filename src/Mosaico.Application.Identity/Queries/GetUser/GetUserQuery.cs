using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Queries.GetUser
{
    [Restricted(nameof(Id), Authorization.Base.Constants.DefaultRoles.Self)]
    public class GetUserQuery : IRequest<GetUserQueryResponse>
    {
        public string Id { get; set; }
    }
}