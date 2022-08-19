using MediatR;
using Mosaico.Application.Identity.Queries.GetUser;

namespace Mosaico.Application.Identity.Queries.FindUser
{
    public class FindUserQuery : IRequest<GetUserQueryResponse>
    {
        public string FindBy { get; set; }
        public string Value { get; set; }
    }
}