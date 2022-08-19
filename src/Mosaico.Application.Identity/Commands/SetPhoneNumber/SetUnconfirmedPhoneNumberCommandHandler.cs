using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Exceptions;

namespace Mosaico.Application.Identity.Commands.SetPhoneNumber
{
    public class SetUnconfirmedPhoneNumberCommandHandler : IRequestHandler<SetUnconfirmedPhoneNumberCommand>
    {
        private readonly IIdentityContext _identityContext;

        public SetUnconfirmedPhoneNumberCommandHandler(IIdentityContext identityContext)
        {
            _identityContext = identityContext;
        }

        public async Task<Unit> Handle(SetUnconfirmedPhoneNumberCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user == null || user.IsDeactivated) throw new UserNotFoundException(request.UserId);
            if (user.PhoneNumberConfirmed == true) throw new PhoneNumberAlreadyConfirmedException(request.UserId);
            user.PhoneNumber = request.PhoneNumber;
            await _identityContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}