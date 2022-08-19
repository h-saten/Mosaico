using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Mosaico.Application.Identity.Abstractions;
using Mosaico.Application.Identity.DTOs;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.Commands.InitiatePasswordChange
{
    public class InitiatePasswordChangeCommandHandler : IRequestHandler<InitiatePasswordChangeCommand, bool>
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly ISecurityCodeGenerator _securityCodeGenerator;
        private readonly ISecurityCodeRepository _codeRepository;
        
        public InitiatePasswordChangeCommandHandler(UserManager<ApplicationUser> userManager, IEventPublisher eventPublisher, IEventFactory eventFactory, ISecurityCodeGenerator securityCodeGenerator, ISecurityCodeRepository codeRepository, ILogger logger = null)
        {
            _userManager = userManager;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _securityCodeGenerator = securityCodeGenerator;
            _codeRepository = codeRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(InitiatePasswordChangeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            var userLogin = await _userManager.CheckPasswordAsync(user, request.OldPassword);

            if (userLogin)
            {
                try
                {
                    _logger?.Verbose($"User {user.Email} was found in database. Initiating password change.");
                    await RemoveAnyExistingCodes(user.Id);
                    var securityCode = await _securityCodeGenerator.GenerateSecurityCodeAsync();
                    var code = await _codeRepository.CreateCodeAsync(securityCode, user.Id, Domain.Identity.Constants.SecurityCodeContexts.PasswordChange);
                    await PublishEventsAsync(user.Id, code.Code, cancellationToken);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger?.Error($"Error occured during sending initiate password change event for user {user.Email}: {ex.Message}");
                    throw;
                }
            }

            throw new IncorrectPasswordException();
        }
        
        private async Task PublishEventsAsync(string id, string code, CancellationToken t)
        {
            var e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, new UserInitiatedPasswordChange(id, code));
            await _eventPublisher.PublishAsync(Events.Identity.Constants.EventPaths.Users, e, t);
        }

        private async Task RemoveAnyExistingCodes(string userId)
        {
            var securityCode = await _codeRepository.GetCodeAsync(userId, Domain.Identity.Constants.SecurityCodeContexts.PasswordChange);
            if (securityCode != null)
            {
                await _codeRepository.SetSecurityCodeUsed(securityCode.Id);
            }
        }
    }
}