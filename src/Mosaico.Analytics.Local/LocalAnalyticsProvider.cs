using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Analytics.Base;
using Mosaico.Analytics.Base.Models;
using Mosaico.Statistics.GoogleAnalytics.Models;

namespace Mosaico.Statistics.Local
{
    public class LocalAnalyticsProvider : ITrafficProvider
    {
        public async Task<PageVisitsCounterDto> PagesVisitsCounterAsync(string projectUrlName, DateTimeOffset start, DateTimeOffset? end = null)
        {
            Random rnd = new Random();
            var visitsAmount = rnd.Next(10000);
            await Task.CompletedTask;
            return new PageVisitsCounterDto
            {
                BuyPageVisits = visitsAmount / 2,
                TokenPageVisits = visitsAmount
            };
        }

        public async Task<PageVisitsDatasetDto> PagesVisitsAsync(string projectUrlName, DateTimeOffset start, DateTimeOffset end)
        {
            Random rnd = new Random();
            var response = new PageVisitsDatasetDto
            {
                FundPageVisits = new List<VisitsDto>(),
                TokenPageVisits = new List<VisitsDto>()
            };
            foreach (DateTime dayFromDateRange in EachDay(start, end))
            {
                var date = dayFromDateRange.ToString("yyyy-MM-dd");
                response.FundPageVisits.Add(new VisitsDto
                {
                    Amount = rnd.Next(10000),
                    Date = date
                });
                response.TokenPageVisits.Add(new VisitsDto
                {
                    Amount = rnd.Next(100000),
                    Date = date
                });
            }

            await Task.CompletedTask;
            return response;
        }
        
        // TODO to extension
        private IEnumerable<DateTime> EachDay(DateTimeOffset from, DateTimeOffset to)
        {
            for(var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}