using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityServer.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Identity.Abstractions;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.Application.Identity.Extensions;
using Mosaico.Authorization.Base;
using Mosaico.BackgroundJobs.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.Commands.ReportStolenAccount
{
    public class ReportStolenAccountCommandHandler : IRequestHandler<ReportStolenAccountCommand>
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
        private readonly ISecurityCodeRepository _codeRepository;
        private readonly ISecurityCodeGenerator _securityCodeGenerator;


        public ReportStolenAccountCommandHandler(IIdentityContext identityContext, UserManager<ApplicationUser> userManager, IEventPublisher eventPublisher, IEventFactory eventFactory, IPersistedGrantService persistedGrantService, SignInManager<ApplicationUser> signInManager, ICurrentUserContext currentUserContext, IBackgroundJobProvider backgroundJobProvider, ISecurityCodeRepository codeRepository, ISecurityCodeGenerator securityCodeGenerator, ILogger logger = null)
        {
            _userManager = userManager;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _logger = logger;
            _securityCodeGenerator = securityCodeGenerator;
            _persistedGrantService = persistedGrantService;
            _signInManager = signInManager;
            _currentUserContext = currentUserContext;
            _identityContext = identityContext;
            _backgroundJobProvider = backgroundJobProvider;
            _codeRepository = codeRepository;
        }

        public async Task<Unit> Handle(ReportStolenAccountCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to deactivate(stolen account) user with {request.Id}");
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user != null)
            {
                var securityCode = await _codeRepository.GetCodeAsync(user.Id, Domain.Identity.Constants.SecurityCodeContexts.AccountStolen);
                if (securityCode == null || securityCode.Code != request.Code)
                {
                    throw new SecurityCodeInvalidException();
                }
                user.IsDeactivated = true;
                user.DeactivatedAt = DateTimeOffset.UtcNow;
                user.DeactivationReason = "Reported as account stolen";
                user.DeactivatedById = Guid.Parse(request.Id);

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    _logger?.Verbose($"Update error occured");
                    _logger?.Verbose(result);
                    throw new RegistrationFailedException(result.Errors?.FirstOrDefault()?.Description);
                }
                await _codeRepository.SetSecurityCodeUsed(securityCode.Id);
                await _persistedGrantService.RemoveAllGrantsAsync(request.Id, "spa-tokenizer");
            }
            
            try
            {
                await PublishEventsAsync(user.Id);
            }
            catch (Exception ex)
            {
                _logger?.Error($"Error occured during publish event about deactivating due to report as stolen user: {ex.Message}/{ex.StackTrace}");
                throw;
            }
            
            _logger?.Verbose($"User {user.Email} was successfully deactivated for stolen report");
            return Unit.Value;
        }

        private async Task PublishEventsAsync(string userId)
        {
            var e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, new UserStolenAccountEvent(userId));
            await _eventPublisher.PublishAsync(e);
        }
    }
}