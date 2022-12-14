using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.Application.Identity.Extensions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.Commands.CreateExternalUser
{
    public class CreateExternalUserCommandHandler : IRequestHandler<CreateExternalUserCommand, Guid>
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityContext _identityContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;

        public CreateExternalUserCommandHandler(
            UserManager<ApplicationUser> userManager, 
            IIdentityContext identityContext, 
            IEventPublisher eventPublisher, 
            IEventFactory eventFactory, 
            ILogger logger = null)
        {
            _userManager = userManager;
            _identityContext = identityContext;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateExternalUserCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to create user {request.Email}");
            
            var normalizedEmail = request.Email.ToUpperInvariant();
            
            var userWithEmailExists = await _identityContext.Users.CountAsync(u => u.NormalizedEmail == normalizedEmail || u.UserName == normalizedEmail, cancellationToken);
            if (userWithEmailExists > 0)
            {
                throw new UserAlreadyExistsException(request.Email);
            }
            _logger?.Verbose($"User with email {request.Email} does not exist");
            
            var user = new ApplicationUser
            {
                Email = request.Email,
                EmailConfirmed = false,
                NormalizedEmail = normalizedEmail,
                UserName = request.Email,
                NormalizedUserName = normalizedEmail,
                AMLStatus = AMLStatus.Unknown,
                TwoFactorEnabled = false,
                Language = Base.Constants.Languages.English
            };
            
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                _logger?.Verbose($"Registration error occured");
                _logger?.Verbose(result);
                throw new RegistrationFailedException(result.Errors?.FirstOrDefault()?.Description);
            }
            
            await _identityContext.SaveChangesAsync(cancellationToken);

            try
            {
                await PublishEventsAsync(user.Id);
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error occured during publish event about newly created user: {ex.Message}/{ex.StackTrace}");
                await _userManager.DeleteAsync(user);
                throw;
            }
            
            _logger?.Verbose($"User {request.Email} was successfully created");
            return Guid.Parse(user.Id);
        }

        private async Task PublishEventsAsync(string userId)
        {
            var e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, new ExternalUserCreatedEvent(userId));
            await _eventPublisher.PublishAsync(Events.Identity.Constants.EventPaths.Users, e);
        }
    }
}