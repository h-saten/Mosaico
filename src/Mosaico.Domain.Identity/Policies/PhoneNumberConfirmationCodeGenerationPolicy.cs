using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.ValueObjects;

namespace Mosaico.Domain.Identity.Policies
{
    public class PhoneNumberConfirmationCodeGenerationPolicy : IPhoneNumberConfirmationCodeGenerationPolicy
    {
        private readonly IIdentityContext _identityContext;
        private readonly IPhoneNumberConfirmationCodesRepository _phoneNumberConfirmationCodesRepository;

        public PhoneNumberConfirmationCodeGenerationPolicy(
            IIdentityContext identityContext, 
            IPhoneNumberConfirmationCodesRepository phoneNumberConfirmationCodesRepository)
        {
            _identityContext = identityContext;
            _phoneNumberConfirmationCodesRepository = phoneNumberConfirmationCodesRepository;
        }

        public async Task<bool> CanGenerate(string userId, PhoneNumber phoneNumberValue)
        {
            var phoneAlreadyInUse = await _identityContext
                .Users
                .AsNoTracking()
                .Where(x => x.PhoneNumber == phoneNumberValue.ToString())
                .AnyAsync();

            if (phoneAlreadyInUse is true)
            {
                return false;
            }
            
            var wasCodeGeneratedInPastMinute =
                await _phoneNumberConfirmationCodesRepository.GetLastlyGeneratedCodeAsync(userId);

            if (wasCodeGeneratedInPastMinute != null)
            {
                return false;
            }
            
            var hourlyLimitDate = DateTimeOffset.UtcNow.AddHours(-1);
            var lastHourCodesCounter = await _identityContext
                .PhoneNumberConfirmationCodes
                .AsNoTracking()
                .Where(x => 
                            x.UserId == userId
                            && x.PhoneNumber.Value == phoneNumberValue.Value 
                            && x.CreatedAt >= hourlyLimitDate)
                .CountAsync();

            if (lastHourCodesCounter >= 3)
            {
                return false;
            }

            return true;
        }
    }
}