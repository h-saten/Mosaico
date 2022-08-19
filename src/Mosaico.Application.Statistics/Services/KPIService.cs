using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Statistics.Abstractions;
using Mosaico.Domain.Statistics.Abstractions;
using Mosaico.Domain.Statistics.Entities;

namespace Mosaico.Application.Statistics.Services
{
    public class KPIService : IKPIService
    {
        private readonly IStatisticsDbContext _statisticsDbContext;

        public KPIService(IStatisticsDbContext statisticsDbContext)
        {
            _statisticsDbContext = statisticsDbContext;
        }

        public async Task<List<KeyValuePair<string, string>>> GetKPIsAsync()
        {
            var kpis = await _statisticsDbContext.KPIs.ToListAsync();
            return kpis.Select(k => new KeyValuePair<string, string>(k.Key, k.Value)).ToList();
        }

        public async Task CreateOrUpdateKPIAsync(string key, string value)
        {
            var existingKPI = await _statisticsDbContext.KPIs.FirstOrDefaultAsync(k => k.Key == key);
            if (existingKPI == null)
            {
                existingKPI = new KPI
                {
                    Key = key,
                    Value = value
                };
                _statisticsDbContext.KPIs.Add(existingKPI);
            }
            else
            {
                existingKPI.Value = value;
                _statisticsDbContext.KPIs.Update(existingKPI);
            }

            await _statisticsDbContext.SaveChangesAsync();
        }
    }
}