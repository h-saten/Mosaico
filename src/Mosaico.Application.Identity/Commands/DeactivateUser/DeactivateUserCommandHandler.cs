using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityServer.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.Application.Identity.Extensions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.Commands.DeactivateUser
{
    [Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]
    public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand>
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IPersistedGrantService _persistedGrantService;
        private readonly IIdentityContext _identityContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICurrentUserContext _currentUserContext;

        public DeactivateUserCommandHandler(UserManager<ApplicationUser> userManager, IEventFactory eventFactory, SignInManager<ApplicationUser> signInManager, IEventPublisher eventPublisher, IPersistedGrantService persistedGrantService, IIdentityContext identityContext, ICurrentUserContext currentUserContext, ILogger logger = null)
        {
            _userManager = userManager;
            _eventFactory = eventFactory;
            _signInManager = signInManager;
            _persistedGrantService = persistedGrantService;
            _eventPublisher = eventPublisher;
            _identityContext = identityContext;
            _currentUserContext = currentUserContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user != null) 
            {
                _logger?.Verbose($"User {user.Email} was found. Attempting to{(request.Status?"deactivate":"activate")} user");
                user.IsDeactivated = request.Status;
                user.DeactivatedAt = DateTimeOffset.UtcNow;
                user.DeactivationReason = request.Reason;
                user.DeactivatedById = Guid.Parse(_currentUserContext.UserId);

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    _logger?.Verbose($"User {(request.Status ? "deactivation" : "activation")} failed");
                    _logger?.Verbose(result);
                    throw new RegistrationFailedException(result.Errors?.FirstOrDefault()?.Description);
                }
                await _persistedGrantService.RemoveAllGrantsAsync(request.Id, "spa-tokenizer");
            }
            try
            {
                await PublishEventsAsync(request.Id, request.Status, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger?.Error($"Failed to send notification about confirmed email for user {request.Id}: {ex.Message}");
            }

            return Unit.Value;
        }
        private async Task PublishEventsAsync(string id, bool status, CancellationToken t)
        {
            CloudEvent e;
            if (status) {
                e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, new UserDeactivated(id));
            }
            else
            {
                e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, new UserActivated(id));
            }
            await _eventPublisher.PublishAsync(Events.Identity.Constants.EventPaths.Users, e, t);
        }
    }
}