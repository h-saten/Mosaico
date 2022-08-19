using MediatR;

namespace Mosaico.Application.Identity.Queries.GetEmailExistence
{
    public class GetEmailExistenceQuery : IRequest<GetEmailExistenceQueryResponse>
    {
        public string Email { get; set; }
    }
}