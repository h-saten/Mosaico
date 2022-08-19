using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;

namespace Mosaico.Application.ProjectManagement.Services
{
    public class UserAffiliationService : IUserAffiliationService
    {
        private readonly IProjectDbContext _projectDbContext;

        public UserAffiliationService(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<UserAffiliation> GetOrCreateUserAffiliation(string userId)
        {
            var userIdNormalized = userId.ToLowerInvariant().Trim();
            var userAffiliation = await _projectDbContext.UserAffiliations.FirstOrDefaultAsync(t => t.UserId == userIdNormalized);
            if (userAffiliation == null)
            {
                userAffiliation = new UserAffiliation
                {
                    UserId = userIdNormalized,
                    AccessCode = await GetUniqueAccessCodeAsync(6)
                };
                await _projectDbContext.UserAffiliations.AddAsync(userAffiliation);
                await _projectDbContext.SaveChangesAsync();
            }

            return userAffiliation;
        }

        private async Task<string> GetUniqueAccessCodeAsync(int length)
        {
            var accessCode = "";
            var str = "";
            var currentAttempt = 0;
            while (true)
            {
                currentAttempt++;
                do 
                {
                    str += Guid.NewGuid().ToString().Replace("-", "");
                } while (length > str.Length);

                accessCode = str[..length];
                var existingAccessCode = await _projectDbContext.UserAffiliations.CountAsync(t => t.AccessCode == accessCode);
                if(existingAccessCode == 0) 
                    break;
                
                if (currentAttempt >= 100)
                {
                    throw new Exception($"Impossible to generate valid access code");
                }
            }
            return accessCode;
        }
    }
}