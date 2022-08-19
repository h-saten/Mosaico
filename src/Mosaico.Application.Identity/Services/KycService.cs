using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Identity.Abstractions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Serilog;

namespace Mosaico.Application.Identity.Services
{
    public class KycService : IKycService
    {
        private readonly IIdentityContext _identityContext;
        private readonly ILogger _logger;

        public KycService(IIdentityContext identityContext, ILogger logger)
        {
            _identityContext = identityContext;
            _logger = logger;
        }

        public async Task SetUserVerifiedAsync(ApplicationUser user, string firstName = null, string lastName = null)
        {
            if (user != null && user.AMLStatus != AMLStatus.Confirmed)
            {
                user.AMLVerifiedAt = DateTimeOffset.UtcNow;
                user.AMLStatus = AMLStatus.Confirmed;
                user.IsAMLVerificationDisabled = true;
                if (!string.IsNullOrWhiteSpace(firstName))
                {
                    user.FirstName = firstName;
                }

                if (!string.IsNullOrWhiteSpace(lastName))
                {
                    user.LastName = lastName;
                }

                await _identityContext.SaveChangesAsync();
            }
        }
        
        public async Task SetUserVerifiedByIdAsync(string userId, string firstName = null, string lastName = null)
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            await SetUserVerifiedAsync(user, firstName, lastName);
        }

        public async Task SetUserVerifiedByEmailAsync(string email, string firstName = null, string lastName = null)
        {
            email = email.Trim().ToUpperInvariant();
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == email);
            await SetUserVerifiedAsync(user, firstName, lastName);
        }

        public async Task SetUserDeclinedAsync(ApplicationUser user)
        {
            if (user != null)
            {
                user.AMLStatus = AMLStatus.Declined;
                user.IsAMLVerificationDisabled = true;
                // user.IsDeactivated = true;
                // user.DeactivationReason = "KYC Declined";
                await _identityContext.SaveChangesAsync();
            }
        }
        
        public async Task SetUserDeclinedByIdAsync(string userId)
        {
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            await SetUserDeclinedAsync(user);
        }

        public async Task SetUserDeclinedByEmailAsync(string email)
        {
            email = email.Trim().ToUpperInvariant();
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == email);
            await SetUserDeclinedAsync(user);
        }
    }
}