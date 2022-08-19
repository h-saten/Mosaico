using MediatR;

namespace Mosaico.Application.Identity.Queries.GetPhoneNumberExistence
{
    public class GetPhoneNumberExistenceQuery : IRequest<GetPhoneNumberExistenceResponse>
    {
        public string PhoneNumber { get; set; }
    }
}