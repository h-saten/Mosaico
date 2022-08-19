using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityServer.Services;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.Application.Identity.Extensions;
using Mosaico.Authorization.Base;
using Mosaico.BackgroundJobs.Base;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IBackgroundJobProvider _backgroundJobProvider;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPersistedGrantService _persistedGrantService;
        private readonly ICurrentUserContext _currentUserContext;


        public DeleteUserCommandHandler(IBackgroundJobProvider backgroundJobProvider, UserManager<ApplicationUser> userManager, IEventPublisher eventPublisher, IEventFactory eventFactory, IPersistedGrantService persistedGrantService, SignInManager<ApplicationUser> signInManager, ICurrentUserContext currentUserContext,  ILogger logger = null)
        {
            _userManager = userManager;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _logger = logger;
            _persistedGrantService = persistedGrantService;
            _signInManager = signInManager;
            _currentUserContext = currentUserContext;
            _backgroundJobProvider = backgroundJobProvider;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            _logger?.Verbose($"Attempting to delete user with {request.Id}");
            var user = await _userManager.FindByIdAsync(request.Id);
            var hasher = new PasswordHasher<ApplicationUser>();
            var verifyHashResult = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (verifyHashResult != PasswordVerificationResult.Success)
            {
                throw new IncorrectPasswordException();
            }
            
            user.MarkedForDeletion = true;
            user.IsDeactivated = true;
            user.DeactivatedAt = DateTimeOffset.UtcNow;
            user.DeactivatedById = Guid.Parse(user.Id);
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.MaxValue;
            var deletionRequest = new DeletionRequest(user.Id, Constants.Jobs.UserAccountDeletionJob)
            {
               DeletionRequestedAt = DateTimeOffset.UtcNow,
               JobName = Constants.Jobs.UserAccountDeletionJob
            };
            user.DeletionRequest = deletionRequest;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger?.Verbose($"Update error occured");
                _logger?.Verbose(result);
                throw new RegistrationFailedException(result.Errors?.FirstOrDefault()?.Description);
            }
            
            await _signInManager.SignOutAsync();
            await _persistedGrantService.RemoveAllGrantsAsync(_currentUserContext.UserId.ToString(), "spa-tokenizer");

            try
            {
                await PublishEventsAsync(user.Id);
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error occured during publish event about deleted user: {ex.Message}/{ex.StackTrace}");
                throw;
            }
            
            _logger?.Verbose($"User {user.Email} was successfully scheduled for deletion");
            return Unit.Value;
        }

        private async Task PublishEventsAsync(string userId)
        {
            var e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, new UserDeleteRequestedEvent(userId));
            await _eventPublisher.PublishAsync(e);
        }
    }
}