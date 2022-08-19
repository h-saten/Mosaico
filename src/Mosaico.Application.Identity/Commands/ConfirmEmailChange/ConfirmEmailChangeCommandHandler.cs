using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.Application.Identity.Extensions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.Commands.ConfirmEmailChange
{
    public class ConfirmEmailChangeCommandHandler : IRequestHandler<ConfirmEmailChangeCommand>
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly ISecurityCodeRepository _codeRepository;

        public ConfirmEmailChangeCommandHandler(UserManager<ApplicationUser> userManager, IEventPublisher eventPublisher, IEventFactory eventFactory, ISecurityCodeRepository codeRepository, ILogger logger = null)
        {
            _userManager = userManager;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _logger = logger;
            _codeRepository = codeRepository;
        }

        public async Task<Unit> Handle(ConfirmEmailChangeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            var oldEmail = user.Email;

            if (user != null)
            {
                _logger?.Verbose($"User {request.UserId} was found. Trying to confirm changed email");

                var result = await _userManager.ChangeEmailAsync(user, request.Email, request.Code);
                if (!result.Succeeded)
                {
                    _logger?.Verbose($"Something went wrong during email change");
                    _logger?.Verbose(result);
                    throw new EmailChangeFailedException("Failed to change email");
                }
                var userNameChangeResult = await _userManager.SetUserNameAsync(user, request.Email);

                if (!userNameChangeResult.Succeeded)
                {
                    _logger?.Verbose($"Something went wrong during email change");
                    _logger?.Verbose(userNameChangeResult);
                    throw new EmailChangeFailedException("Failed to change email");
                }
                 var securityCode = _userManager.GenerateNewAuthenticatorKey();
                var code = await _codeRepository.CreateCodeAsync(securityCode, user.Id, Domain.Identity.Constants.SecurityCodeContexts.AccountStolen);
                _logger?.Verbose($"User {request.UserId} successfully changed email to {request.Email}");
                _logger?.Verbose($"Sending events");
                await PublishEventsAsync(user.Id, oldEmail,code.Code);
            }
            return Unit.Value;
        }
        
        private async Task PublishEventsAsync(string userId, string OldEmail, string code)
        {
            var e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, new UserEmailChanged(userId, OldEmail,code));
            await _eventPublisher.PublishAsync(Events.Identity.Constants.EventPaths.Users, e);
        }
    }
}