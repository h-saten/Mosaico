using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.ValueObjects;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Serilog;

namespace Mosaico.Application.Identity.Commands.VerifyPhoneNumber
{
    public class VerifyPhoneNumberCommandHandler : IRequestHandler<VerifyPhoneNumberCommand>
    {
        private readonly ILogger _logger;
        private readonly IIdentityContext _identityContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly ISecurityCodeRepository _codeRepository;
        private readonly IPhoneNumberConfirmationCodesRepository _phoneNumberConfirmationCodesRepository;
        
        public VerifyPhoneNumberCommandHandler(
            IIdentityContext identityContext,
            UserManager<ApplicationUser> userManager, 
            IEventPublisher eventPublisher, 
            IEventFactory eventFactory,
            ISecurityCodeRepository codeRepository,
            IPhoneNumberConfirmationCodesRepository phoneNumberConfirmationCodesRepository, 
            ILogger logger = null)
        {
            _identityContext = identityContext;
            _userManager = userManager;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _codeRepository = codeRepository;
            _phoneNumberConfirmationCodesRepository = phoneNumberConfirmationCodesRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(VerifyPhoneNumberCommand request, CancellationToken cancellationToken)
        {
            await using var dbTransaction = _identityContext.BeginTransaction();
            var user = await _userManager.FindByIdAsync(request.UserId);
            var confirmationCode = request.ConfirmationCode;
            var phoneNumber = new PhoneNumber(request.PhoneNumber);

            var code = await _phoneNumberConfirmationCodesRepository.GetLastlyGeneratedCodeAsync(request.UserId);
            
            if (code == null || code.Code != request.ConfirmationCode)
            {
                throw new InvalidPhoneNumberConfirmationCodeException(phoneNumber, confirmationCode);
            }

            await _phoneNumberConfirmationCodesRepository.SetSecurityCodeUsed(code.Id);
            user.PhoneNumberConfirmed = true;
            user.PhoneNumber = request.PhoneNumber;
            code.MarkAsUsed();
            _identityContext.PhoneNumberConfirmationCodes.Update(code);
            _identityContext.Users.Update(user);
            await _identityContext.SaveChangesAsync(cancellationToken);
            await dbTransaction.CommitAsync(cancellationToken);
            var stolenSecurityCode = _userManager.GenerateNewAuthenticatorKey();
            var securityCode = await _codeRepository.CreateCodeAsync(stolenSecurityCode, user.Id, Domain.Identity.Constants.SecurityCodeContexts.AccountStolen);
            await PublishEventsAsync(user.Id, securityCode.Code);
            return Unit.Value;
        }
        
        private async Task PublishEventsAsync(string userId, string code=null)
        {
            var e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, new UserPhoneNumberVerified(userId,code));
            await _eventPublisher.PublishAsync(Events.Identity.Constants.EventPaths.Users, e);
        }
    }
}