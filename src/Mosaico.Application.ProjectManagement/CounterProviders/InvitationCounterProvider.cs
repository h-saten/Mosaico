using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Core.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;

namespace Mosaico.Application.ProjectManagement.CounterProviders
{
    public class InvitationCounterProvider : ICounterProvider
    {
        private readonly IProjectDbContext _projectDbContext;

        public InvitationCounterProvider(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<KeyValuePair<string, int>> GetCountersAsync(string userId)
        {
            var now = DateTimeOffset.UtcNow;
            var projectsCount = await _projectDbContext.ProjectMembers.AsNoTracking().CountAsync(p => p.UserId == userId && !p.IsAccepted && p.ExpiresAt > now);
            return new KeyValuePair<string, int>(Constants.Counters.Invitations, projectsCount);
        }
    }
}