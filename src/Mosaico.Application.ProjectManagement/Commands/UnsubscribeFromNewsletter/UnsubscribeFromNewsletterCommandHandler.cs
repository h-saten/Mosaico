using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Extensions;
using Org.BouncyCastle.Security;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.UnsubscribeFromNewsletter
{
    public class UnsubscribeFromNewsletterCommandHandler : IRequestHandler<UnsubscribeFromNewsletterCommand>
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _dbContext;
        private readonly ICurrentUserContext _currentUserContext;

        public UnsubscribeFromNewsletterCommandHandler(IProjectDbContext dbContext, ICurrentUserContext currentUserContext, ILogger logger = null)
        {
            _dbContext = dbContext;
            _currentUserContext = currentUserContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(UnsubscribeFromNewsletterCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to unsubscribe from newsletter");
            var userEmail = request.Email;
            var project = await _dbContext.GetProjectOrThrowAsync(request.ProjectId, cancellationToken);
            if (!string.IsNullOrWhiteSpace(request.UserId))
            {
                _logger?.Verbose($"User ID was supplied. Trying to identify the user");
                _logger?.Verbose($"User ID was supplied. Trying to validate user");
                if (!string.Equals(request.UserId, _currentUserContext.UserId, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new ForbiddenException();
                }

                userEmail = _currentUserContext.Email;
            }
            
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                throw new InvalidParameterException("email");
            }
            
            var normalizedEmail = userEmail.ToUpperInvariant();
            
            var subscription = project.ProjectNewsletterSubscriptions.FirstOrDefault(s => s.NormalizedEmail == normalizedEmail);
            if (subscription == null)
            {
                throw new SubscriptionNotFoundException(request.Email);
            }

            if (string.IsNullOrWhiteSpace(subscription.UserId))
            {
                if (!string.Equals(request.Code.ToUpperInvariant(), subscription.Code.ToUpperInvariant()))
                {
                    throw new ForbiddenException();
                }
            }

            _dbContext.ProjectNewsletterSubscriptions.Remove(subscription);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}