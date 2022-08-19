using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;

namespace Mosaico.Application.Identity.Commands.InitKycVerification
{
    public class InitKycVerificationCommandHandler : IRequestHandler<InitKycVerificationCommand, Guid>
    {
        private readonly IIdentityContext _identityContext;

        public InitKycVerificationCommandHandler(IIdentityContext identityContext)
        {
            _identityContext = identityContext;
        }

        public async Task<Guid> Handle(InitKycVerificationCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);
            if (user == null)
            {
                throw new UserNotFoundException(request.UserId);
            }
            var previousVerifications =
                await _identityContext.KycVerifications.Where(t => t.UserId == request.UserId).ToListAsync(cancellationToken);
            
            if (previousVerifications.Count > 3)
            {
                throw new LimitExceededException(nameof(KycVerification));
            }

            var verification = new KycVerification
            {
                Provider = request.Provider,
                Status = KycVerificationStatus.Pending,
                UserId = request.UserId,
                TransactionId = request.Id
            };
            _identityContext.KycVerifications.Add(verification);
            user.AMLStatus = AMLStatus.Pending;
            await _identityContext.SaveChangesAsync(cancellationToken);
            return verification.Id;
        }
    }
}