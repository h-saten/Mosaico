using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Mosaico.Application.Identity.Extensions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.Commands.ConfirmChangePassword
{
    public class ConfirmPasswordChangeCommandHandler : IRequestHandler<ConfirmPasswordChangeCommand>
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityContext _identityContext;
        private readonly ISecurityCodeRepository _codeRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;

        public ConfirmPasswordChangeCommandHandler(UserManager<ApplicationUser> userManager, IIdentityContext identityContext, ISecurityCodeRepository codeRepository, IEventPublisher eventPublisher, IEventFactory eventFactory, ILogger logger = null)
        {
            _userManager = userManager;
            _identityContext = identityContext;
            _codeRepository = codeRepository;
            _logger = logger;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
        }

        public async Task<Unit> Handle(ConfirmPasswordChangeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user != null)
            {
                _logger?.Verbose($"User {user.Email} was found. Attempting to set new password");
                var securityCode = await _codeRepository.GetCodeAsync(user.Id, Domain.Identity.Constants.SecurityCodeContexts.PasswordChange);
                if (securityCode == null || securityCode.Code != request.Code)
                {
                    throw new SecurityCodeInvalidException();
                }
                
                var resultChangePassword = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
                var resultUpdateSecurityStamp = await _userManager.UpdateSecurityStampAsync(user);
                if (resultChangePassword.Succeeded && resultUpdateSecurityStamp.Succeeded)
                {
                    var emailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                    if (!emailConfirmed)
                    {
                        user.EmailConfirmed = true;
                        await _identityContext.SaveChangesAsync(cancellationToken);
                    }

                    await _codeRepository.SetSecurityCodeUsed(securityCode.Id);
                }
                else
                {
                    _logger?.Verbose($"Something went wrong during password reset");
                    _logger?.Verbose(resultChangePassword);
                    _logger?.Verbose(resultUpdateSecurityStamp);
                }
                var stolenSecurityCode = _userManager.GenerateNewAuthenticatorKey();
                var code = await _codeRepository.CreateCodeAsync(stolenSecurityCode, user.Id, Domain.Identity.Constants.SecurityCodeContexts.AccountStolen);
                await PublishEventsAsync(user.Id, user.Email, code.Code);
            }
             
            return Unit.Value;
        }
        private async Task PublishEventsAsync(string userId, string email, string code)
        {
            var e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, new UserConfirmedPasswordChange(userId, email, code));
            await _eventPublisher.PublishAsync(Events.Identity.Constants.EventPaths.Users, e);
        }
    }
}