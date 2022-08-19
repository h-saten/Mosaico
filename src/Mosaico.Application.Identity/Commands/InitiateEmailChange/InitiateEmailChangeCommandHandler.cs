using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.Commands.InitiateEmailChange
{
    public class InitiateEmailChangeCommandHandler : IRequestHandler<InitiateEmailChangeCommand>
    {
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityContext _identityContext;

        public InitiateEmailChangeCommandHandler(UserManager<ApplicationUser> userManager,
            IEventPublisher eventPublisher, IEventFactory eventFactory, IIdentityContext identityContext, ILogger logger = null)
        {
            _userManager = userManager;
            _logger = logger;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _identityContext = identityContext;
        }

        public async Task<Unit> Handle(InitiateEmailChangeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user != null)
            {
                _logger?.Information($"User {request.Email} was found in database. Initiating email change event.");
                var normalizedNewEmail = request.Email.Trim().ToUpperInvariant();
                var otherUsersWithThisEmailCount = await _identityContext.Users.CountAsync(u => u.NormalizedEmail == normalizedNewEmail, cancellationToken);
                
                if (otherUsersWithThisEmailCount > 0)
                {
                    throw new EmailChangeFailedException("Failed to change email");
                }
                
                var passwordValidatedUser = await _userManager.CheckPasswordAsync(user, request.Password);

                if (passwordValidatedUser)
                {
                    await PublishEventsAsync(user.Id, request.Email, cancellationToken);
                }
                else
                {
                    throw new EmailChangeFailedException("Failed to change email");
                }
            }
            else
            {
                _logger?.Verbose($"User {request.UserId} was not found");
            }

            return Unit.Value;
        }

        private async Task PublishEventsAsync(string id, string email, CancellationToken t)
        {
            var e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users,
                new UserInitiatedEmailChange(id, email));
            await _eventPublisher.PublishAsync(Events.Identity.Constants.EventPaths.Users, e, t);
        }
    }
}