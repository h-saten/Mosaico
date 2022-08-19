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

namespace Mosaico.Application.Identity.Commands.UpdatePhoneNumber
{
    public class UpdatePhoneNumberCommandHandler : IRequestHandler<UpdatePhoneNumberCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityContext _identityContext;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ISecurityCodeRepository _codeRepository;
        private readonly IPhoneNumberConfirmationCodesRepository _phoneNumberConfirmationCodesRepository;

        public UpdatePhoneNumberCommandHandler(
            UserManager<ApplicationUser> userManager, 
            IIdentityContext identityContext, 
            IEventFactory eventFactory, 
            IEventPublisher eventPublisher,
            ISecurityCodeRepository codeRepository,
            IPhoneNumberConfirmationCodesRepository phoneNumberConfirmationCodesRepository)
        {
            _userManager = userManager;
            _identityContext = identityContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _codeRepository = codeRepository;
            _phoneNumberConfirmationCodesRepository = phoneNumberConfirmationCodesRepository;
        }

        public async Task<Unit> Handle(UpdatePhoneNumberCommand request, CancellationToken cancellationToken)
        {
            await using var dbTransaction = _identityContext.BeginTransaction();
            var user = await _userManager.FindByIdAsync(request.UserId);
            var confirmationCode = request.Code;
            var phoneNumber = new PhoneNumber(request.PhoneNumber);
            var code = await _phoneNumberConfirmationCodesRepository.GetLastlyGeneratedCodeAsync(user.Id);
            var isCodeVerifiedSuccessfully = code is not null && code.Code == request.Code;
            if (isCodeVerifiedSuccessfully is false)
            {
                throw new InvalidPhoneNumberConfirmationCodeException(phoneNumber, confirmationCode);
            }
            
            code.MarkAsUsed();
            
            _identityContext.PhoneNumberConfirmationCodes.Update(code);
            user.PhoneNumberConfirmed = true;
            user.PhoneNumber = request.PhoneNumber;
            _identityContext.Users.Update(user);
            await _identityContext.SaveChangesAsync(cancellationToken);
            await dbTransaction.CommitAsync(cancellationToken);
            var stolenSecurityCode = _userManager.GenerateNewAuthenticatorKey();
            var SecurityCode = await _codeRepository.CreateCodeAsync(stolenSecurityCode, user.Id, Domain.Identity.Constants.SecurityCodeContexts.AccountStolen);
            await PublishEventsAsync(user.Id, SecurityCode.Code);
            return Unit.Value;
        }
        
        private async Task PublishEventsAsync(string userId, string code = null)
        {
            var e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, new UserPhoneNumberVerified(userId,code));
            await _eventPublisher.PublishAsync(Events.Identity.Constants.EventPaths.Users, e);
        }
    }
}