using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;
using Mosaico.Domain.Identity.ValueObjects;

namespace Mosaico.Domain.Identity.Repositories
{
    public class PhoneNumberConfirmationCodesRepository : IPhoneNumberConfirmationCodesRepository
    {
        private readonly IIdentityContext _identityContext;

        public PhoneNumberConfirmationCodesRepository(IIdentityContext identityContext)
        {
            _identityContext = identityContext;
        }

        public Task<PhoneNumberConfirmationCode> GetLastlyGeneratedCodeAsync(string userId)
        {
            var currentDate = DateTimeOffset.UtcNow;
            return _identityContext.PhoneNumberConfirmationCodes.Where(s => s.ExpiresAt > currentDate && !s.Used)
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<PhoneNumberConfirmationCode> CreateCodeAsync(string userId, PhoneNumber phoneNumber, string codeValue = null)
        {
            var code = new PhoneNumberConfirmationCode(userId, phoneNumber, codeValue);
            _identityContext.PhoneNumberConfirmationCodes.Add(code);
            await _identityContext.SaveChangesAsync();
            return code;
        }

        public async Task SetSecurityCodeUsed(Guid id)
        {
            var securityCode = await _identityContext.PhoneNumberConfirmationCodes.FirstOrDefaultAsync(s => s.Id == id);
            if (securityCode == null)
            {
                throw new PhoneNumberConfirmationCodeInvalidException();
            }
            else
            {
                securityCode.MarkAsUsed();
                _identityContext.PhoneNumberConfirmationCodes.Update(securityCode);
                await _identityContext.SaveChangesAsync();
            }
        }
    }
}