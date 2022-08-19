using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Exceptions;

namespace Mosaico.Application.Identity.Queries.GetUserKycStatus
{
    public class GetUserKycStatusQueryHandler : IRequestHandler<GetUserKycStatusQuery, string>
    {
        private readonly IIdentityContext _identityContext;

        public GetUserKycStatusQueryHandler(IIdentityContext identityContext)
        {
            _identityContext = identityContext;
        }

        public async Task<string> Handle(GetUserKycStatusQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user == null)
            {
                throw new UserNotFoundException(request.UserId);
            }

            return user.AMLStatus.ToString();
        }
    }
}