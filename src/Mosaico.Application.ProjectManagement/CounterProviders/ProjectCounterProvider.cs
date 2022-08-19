using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Core.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;

namespace Mosaico.Application.ProjectManagement.CounterProviders
{
    public class ProjectCounterProvider : ICounterProvider
    {
        private readonly IProjectDbContext _projectDbContext;

        public ProjectCounterProvider(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<KeyValuePair<string, int>> GetCountersAsync(string userId)
        {
            var projectsCount = await _projectDbContext.ProjectMembers.AsNoTracking().CountAsync(p => p.UserId == userId && p.IsAccepted);
            return new KeyValuePair<string, int>(Constants.Counters.Projects, projectsCount);
        }
    }
}