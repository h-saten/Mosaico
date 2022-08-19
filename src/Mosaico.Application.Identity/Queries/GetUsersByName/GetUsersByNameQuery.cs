using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Queries.GetUsersByName
{
    public class GetUsersByNameQuery : IRequest<GetUsersByNameQueryResponse>
    {
        public string UserName { get; set; } = "null";
    }
}