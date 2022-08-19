using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.ValueObjects;

namespace Mosaico.Application.Identity.Queries.GetPhoneNumberExistence
{
    public class GetPhoneNumberExistenceQueryHandler : IRequestHandler<GetPhoneNumberExistenceQuery, GetPhoneNumberExistenceResponse>
    {
        private readonly IIdentityContext _identityContext;

        public GetPhoneNumberExistenceQueryHandler(IIdentityContext identityContext)
        {
            _identityContext = identityContext;
        }

        public async Task<GetPhoneNumberExistenceResponse> Handle(GetPhoneNumberExistenceQuery request, CancellationToken cancellationToken)
        {
            var phone = new PhoneNumber(request.PhoneNumber);
            
            var phoneAlreadyInUse = await _identityContext
                .Users
                .AsNoTracking()
                .Where(x => x.PhoneNumber == phone.ToString())
                .AnyAsync(cancellationToken);
            
            return new GetPhoneNumberExistenceResponse
            {
                Exist = phoneAlreadyInUse
            };
        }
    }
}