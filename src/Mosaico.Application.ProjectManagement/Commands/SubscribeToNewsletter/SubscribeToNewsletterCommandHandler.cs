using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Authorization.Base;
using Mosaico.Base.Abstractions;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Extensions;
using Org.BouncyCastle.Security;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.SubscribeToNewsletter
{
    public class SubscribeToNewsletterCommandHandler : IRequestHandler<SubscribeToNewsletterCommand>
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _dbContext;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IStringHasher _stringHasher;

        public SubscribeToNewsletterCommandHandler(IProjectDbContext dbContext, ICurrentUserContext currentUserContext, IStringHasher stringHasher, ILogger logger = null)
        {
            _dbContext = dbContext;
            _currentUserContext = currentUserContext;
            _stringHasher = stringHasher;
            _logger = logger;
        }

        public async Task<Unit> Handle(SubscribeToNewsletterCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to subscribe to the newsletter");
            var userEmail = request.Email;

            var project = await _dbContext.GetProjectOrThrowAsync(request.ProjectId, cancellationToken);
            
            if (!string.IsNullOrWhiteSpace(request.UserId))
            {
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
            
            var subscription = project.ProjectNewsletterSubscriptions.FirstOrDefault(t => t.NormalizedEmail == normalizedEmail);
            
            if (subscription != null)
            {
                throw new AlreadySubscribedToNewsletterException(userEmail);
            }
            
            subscription = new ProjectNewsletterSubscription
            {
                Email = userEmail,
                ProjectId = project.Id,
                Project = project,
                UserId = request.UserId,
                SubscribedAt = DateTimeOffset.UtcNow,
                NormalizedEmail = normalizedEmail,
                Code = _stringHasher.CreateHash(Guid.NewGuid().ToString())
            };
            _dbContext.ProjectNewsletterSubscriptions.Add(subscription);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}