using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityServer.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.Application.Identity.Extensions;
using Mosaico.Authorization.Base;
using Mosaico.BackgroundJobs.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.Commands.RestoreUser
{
    public class RestoreUserCommandHandler : IRequestHandler<RestoreUserCommand>
    {
        private readonly IBackgroundJobProvider _backgroundJobProvider;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPersistedGrantService _persistedGrantService;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IIdentityContext _identityContext;


        public RestoreUserCommandHandler(IIdentityContext identityContext, UserManager<ApplicationUser> userManager, IEventPublisher eventPublisher, IEventFactory eventFactory, IPersistedGrantService persistedGrantService, SignInManager<ApplicationUser> signInManager, ICurrentUserContext currentUserContext, IBackgroundJobProvider backgroundJobProvider,  ILogger logger = null)
        {
            _userManager = userManager;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _logger = logger;
            _persistedGrantService = persistedGrantService;
            _signInManager = signInManager;
            _currentUserContext = currentUserContext;
            _identityContext = identityContext;
            _backgroundJobProvider = backgroundJobProvider;
        }

        public async Task<Unit> Handle(RestoreUserCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to restore user with {request.Id}");
            var user = await _userManager.FindByIdAsync(request.Id);

            user.MarkedForDeletion = false;
            user.IsDeactivated = false;
            user.DeactivatedAt = null;
            user.DeactivatedById = null;
            user.LockoutEnabled = false;
            user.LockoutEnd = null;
            user.EmailConfirmed = false;
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, Guid.NewGuid().ToString());
            var deletionRequest = await _identityContext.DeletionRequests.FirstOrDefaultAsync(x => x.UserId == request.Id, cancellationToken);
            if (deletionRequest != null)
            {
                _identityContext.DeletionRequests.Remove(deletionRequest);
            }
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger?.Verbose($"Update error occured");
                _logger?.Verbose(result);
                throw new RegistrationFailedException(result.Errors?.FirstOrDefault()?.Description);
            }
            
            try
            {
                await PublishEventsAsync(user.Id);
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error occured during publish event about restoring user: {ex.Message}/{ex.StackTrace}");
                throw;
            }
            
            _logger?.Verbose($"User {user.Email} was successfully restored");
            return Unit.Value;
        }

        private async Task PublishEventsAsync(string userId)
        {
            var e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, new UserRestoredEvent(userId));
            await _eventPublisher.PublishAsync(e);
        }
    }
}