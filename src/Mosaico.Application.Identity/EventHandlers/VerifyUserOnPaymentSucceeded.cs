using System;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Identity.Abstractions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Serilog;

namespace Mosaico.Application.Identity.EventHandlers
{
    [EventInfo(nameof(VerifyUserOnPaymentSucceeded),  "wallets:id")]
    [EventTypeFilter(typeof(SuccessfulPurchaseEvent))]
    public class VerifyUserOnPaymentSucceeded : EventHandlerBase
    {
        private readonly IIdentityContext _identityContext;
        private readonly IIdentityEmailService _emailService;
        private readonly IKycService _kycService;
        private readonly ILogger _logger;
        
        public VerifyUserOnPaymentSucceeded(IIdentityContext identityContext, IIdentityEmailService emailService, IKycService kycService, ILogger logger = null)
        {
            _identityContext = identityContext;
            _emailService = emailService;
            _kycService = kycService;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<SuccessfulPurchaseEvent>();
            if (data != null)
            {
                if(data.PaymentProcessor == "RAMP" || data.PaymentProcessor == "TRANSAK")
                {
                    var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == data.UserId);
                    if(user != null && user.AMLStatus != AMLStatus.Confirmed)
                    {
                        await _kycService.SetUserVerifiedAsync(user);
                        await SendSuccessfulEmailAsync(user.Email, user.Language);
                    }
                }
            }
        }
        
        private async Task SendSuccessfulEmailAsync(string email, string language)
        {
            try
            {
                await _emailService.SendKycCompletedSuccessfullyAsync(email, language);
            }
            catch(Exception ex)
            {
                _logger?.Warning($"Exception during sending email about KYC: {ex.Message} / {ex.StackTrace}");
            }
        }
    }
}