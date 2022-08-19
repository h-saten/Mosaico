using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;

namespace Mosaico.Domain.Identity.Repositories
{
    public class SecurityCodeRepository : ISecurityCodeRepository
    {
        private readonly IIdentityContext _identityContext;

        public SecurityCodeRepository(IIdentityContext identityContext)
        {
            _identityContext = identityContext;
        }

        public Task<SecurityCode> GetCodeAsync(string userId, string context)
        {
            var now = DateTimeOffset.UtcNow;
            return _identityContext
                .SecurityCodes
                .Where(s => s.ExpiresAt > now && !s.IsUsed)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(s => s.UserId == userId && s.Context == context && s.IsUsed == false);
        }

        public async Task<SecurityCode> CreateCodeAsync(string code, string userId, string context)
        {
            var securityCode = new SecurityCode
            {
                Code = code,
                UserId = userId,
                Context = context,
                IsUsed = false,
                ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(Constants.SecurityCodeExpiresInMinutes)
            };
            _identityContext.SecurityCodes.Add(securityCode);
            await _identityContext.SaveChangesAsync();
            return securityCode;
        }

        public async Task SetSecurityCodeUsed(Guid id)
        {
            var securityCode = await _identityContext.SecurityCodes.FirstOrDefaultAsync(s => s.Id == id);
            if (securityCode == null)
            {
                throw new SecurityCodeInvalidException();
            }
            else
            {
                securityCode.IsUsed = true;
                securityCode.ExpiresAt = DateTimeOffset.UtcNow;
                await _identityContext.SaveChangesAsync();
            }
        }
    }
}