using System.Threading.Tasks;
using Google.Apis.AnalyticsReporting.v4;

namespace Mosaico.Statistics.GoogleAnalytics.Abstractions
{
    public interface IAnalyticsReportingConnector
    {
        Task<AnalyticsReportingService> Connect();
    }
}