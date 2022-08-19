using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Core.Abstractions;
using Mosaico.Domain.BusinessManagement.Abstractions;

namespace Mosaico.Application.BusinessManagement.CounterProviders
{
    public class CompanyCounterProvider : ICounterProvider
    {
        private readonly IBusinessDbContext _businessDb;

        public CompanyCounterProvider(IBusinessDbContext businessDb)
        {
            _businessDb = businessDb;
        }

        public async Task<KeyValuePair<string, int>> GetCountersAsync(string userId)
        {
            var companyCount = await _businessDb.TeamMembers.AsNoTracking().CountAsync(p => p.UserId == userId);
            return new KeyValuePair<string, int>(Constants.Counters.Companies, companyCount);
        }
    }
}