using System;
using System.Threading.Tasks;
using Mosaico.Analytics.Base.Models;
using Mosaico.Statistics.GoogleAnalytics.Models;

namespace Mosaico.Analytics.Base
{
    public interface ITrafficProvider
    {
        Task<PageVisitsCounterDto> PagesVisitsCounterAsync
            (string projectUrlName, DateTimeOffset start, DateTimeOffset? end = null);
        Task<PageVisitsDatasetDto> PagesVisitsAsync(string projectUrlName, DateTimeOffset start, DateTimeOffset end);
    }
}