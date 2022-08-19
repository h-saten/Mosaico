using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Identity.Abstractions;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.KYC.Passbase.Abstractions;
using Serilog;

namespace Mosaico.Application.Identity.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.UserKycVerificationJob, IsRecurring = true, Cron = "*/5 * * * *")]
    public class UserKycVerificationJob : HangfireBackgroundJobBase
    {
        private readonly IIdentityContext _identityContext;
        private readonly ILogger _logger;
        private readonly IIdentityEmailService _emailService;
        private readonly IKycService _kycService;
        private readonly IPassbaseClient _passbaseClient;

        public UserKycVerificationJob(IIdentityContext identityContext, ILogger logger, IIdentityEmailService emailService, IPassbaseClient passbaseClient, IKycService kycService)
        {
            _identityContext = identityContext;
            _logger = logger;
            _emailService = emailService;
            _passbaseClient = passbaseClient;
            _kycService = kycService;
        }

        public override async Task ExecuteAsync(object parameters = null)
        {
            var basisIdVerifications = await _identityContext.KycVerifications.Where(t =>
                t.Status == KycVerificationStatus.Pending)
                .ToListAsync();
            foreach (var verification in basisIdVerifications)
            {
                try
                {
                    if (verification.Provider == KYC.Passbase.Constants.ProviderName)
                    {
                        await HandlePassbaseAsync(verification);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Warning($"{ex.Message} / {ex.StackTrace}");
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
        
        private async Task HandlePassbaseAsync(KycVerification verification)
        {
            var identity = await _passbaseClient.GetIdentityAsync(verification.TransactionId);
            if(identity != null)
            {
                var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == verification.UserId);
                if (user != null)
                {
                    if (identity.Status == "approved" && user.AMLStatus != AMLStatus.Confirmed)
                    {
                        await _kycService.SetUserVerifiedByIdAsync(verification.UserId, identity?.Owner?.FirstName, identity?.Owner?.LastName);
                        verification.Status = KycVerificationStatus.Verified;
                        await _identityContext.SaveChangesAsync();
                        await SendSuccessfulEmailAsync(user.Email, user.Language);
                    }
                    else if (identity.Status == "declined")
                    {
                        await _kycService.SetUserDeclinedByIdAsync(verification.UserId);
                        verification.Status = KycVerificationStatus.Failed;
                        await _identityContext.SaveChangesAsync();
                    }
                }
            }
        }
    }
}