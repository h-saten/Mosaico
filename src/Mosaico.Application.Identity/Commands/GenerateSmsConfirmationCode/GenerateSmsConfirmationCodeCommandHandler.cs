using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Policies;
using Mosaico.Domain.Identity.ValueObjects;
using Mosaico.Integration.Sms.Abstraction;
using Serilog;

namespace Mosaico.Application.Identity.Commands.GenerateSmsConfirmationCode
{
    public class GenerateSmsConfirmationCodeCommandHandler : IRequestHandler<GenerateSmsConfirmationCodeCommand>
    {
        private readonly ILogger _logger;
        private readonly IIdentityContext _identityContext;
        private readonly IPhoneNumberConfirmationCodeGenerationPolicy _phoneNumberConfirmationCodeGenerationPolicy;
        private readonly IPhoneNumberConfirmationCodesRepository _phoneNumberConfirmationCodesRepository;
        private readonly ISmsSender _smsSender;
        
        public GenerateSmsConfirmationCodeCommandHandler(
            IIdentityContext identityContext, 
            IPhoneNumberConfirmationCodeGenerationPolicy phoneNumberConfirmationCodeGenerationPolicy, 
            ISmsSender smsSender, 
            IPhoneNumberConfirmationCodesRepository phoneNumberConfirmationCodesRepository, 
            ILogger logger = null)
        {
            _identityContext = identityContext;
            _phoneNumberConfirmationCodeGenerationPolicy = phoneNumberConfirmationCodeGenerationPolicy;
            _smsSender = smsSender;
            _phoneNumberConfirmationCodesRepository = phoneNumberConfirmationCodesRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(GenerateSmsConfirmationCodeCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Command: '{nameof(GenerateSmsConfirmationCodeCommand)} execution started.'");
            var phoneNumber = new PhoneNumber(request.PhoneNumber);
            var canGenerateCode = await _phoneNumberConfirmationCodeGenerationPolicy.CanGenerate(request.UserId, phoneNumber);
            if (canGenerateCode is false)
            {
                throw new ConfirmationCodeCannotBeGeneratedException(phoneNumber);
            }

            await using var dbTransaction = _identityContext.BeginTransaction();
            var confirmationCode = await _phoneNumberConfirmationCodesRepository
                .CreateCodeAsync(request.UserId, new PhoneNumber(request.PhoneNumber));

            _logger?.Verbose($"Sms sending action.'");
            await _smsSender.SendAsync(new SmsMessage
            {
                Content = $"Your confirmation code: {confirmationCode.Code}.",
                Subject = "Number ownership confirmation",
                RecipientsPhoneNumber = phoneNumber.ToString()
            }, cancellationToken);
            _logger?.Verbose($"Sms sent successfully.'");
            
            await dbTransaction.CommitAsync(cancellationToken);
            _logger?.Verbose($"Db transaction committed.'");
            
            return Unit.Value;
        }
    }
}