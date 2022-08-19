using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Exceptions;
using Serilog;

namespace Mosaico.Application.Identity.Commands.SetUserLanguage
{
    public class SetUserLanguageCommandHandler : IRequestHandler<SetUserLanguageCommand>
    {
        private readonly ILogger _logger;
        private readonly IIdentityContext _identityContext;

        public SetUserLanguageCommandHandler(IIdentityContext identityContext, ILogger logger = null)
        {
            _logger = logger;
            _identityContext = identityContext;
        }

        public async Task<Unit> Handle(SetUserLanguageCommand request, CancellationToken cancellationToken)
        {
            var userId = request.UserId.ToString();
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null)
            {
                throw new UserNotFoundException(request.UserId);
            }

            user.Language = request.Language;
            await _identityContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}