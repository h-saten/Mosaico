using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.AnalyticsReporting.v4.Data;
using Mosaico.Analytics.Base;
using Mosaico.Analytics.Base.Models;
using Mosaico.Base.Abstractions;
using Mosaico.Statistics.GoogleAnalytics.Abstractions;
using Mosaico.Statistics.GoogleAnalytics.Configuration;
using Mosaico.Statistics.GoogleAnalytics.Models;

namespace Mosaico.Statistics.GoogleAnalytics
{
    public class GoogleAnalyticsProjectStatistics : ITrafficProvider
    {

        private readonly IAnalyticsReportingConnector _analyticsReportingConnector;
        private readonly GoogleAnalyticsConfiguration _googleAnalyticsApiConfig;
        
        
        public GoogleAnalyticsProjectStatistics(
            IAnalyticsReportingConnector analyticsReportingConnector, 
            GoogleAnalyticsConfiguration googleAnalyticsApiConfig)
        {
            _analyticsReportingConnector = analyticsReportingConnector;
            _googleAnalyticsApiConfig = googleAnalyticsApiConfig;
        }

        private async Task<AnalyticsReportingService> Connect()
        {
            return await _analyticsReportingConnector.Connect();
        }
        
        public async Task<PageVisitsCounterDto> PagesVisitsCounterAsync
            (string projectUrlName, DateTimeOffset start, DateTimeOffset? end = null)
        {
            var service = await Connect();

            var now = end ?? DateTime.UtcNow;
            var endDate = now.ToString("yyyy-MM-dd");
            
            string startDate = start.ToString("yyyy-MM-dd");

            var dateRange = new DateRange {StartDate = startDate, EndDate = endDate};

            var uniquePageviews = new Metric {Expression = "ga:uniquePageviews", Alias = "Unique Pageviews"};

            var tokenPageVisitsRequest = new ReportRequest
            {
                ViewId = _googleAnalyticsApiConfig.ReportViewId,
                DateRanges = new List<DateRange> {dateRange},
                Metrics = new List<Metric> {uniquePageviews},
                FiltersExpression = $"ga:pagePath=~/project/{projectUrlName}/.*"
            };
            
            var buyPageVisitsRequest = new ReportRequest
            {
                ViewId = _googleAnalyticsApiConfig.ReportViewId,
                DateRanges = new List<DateRange> {dateRange},
                Metrics = new List<Metric> {uniquePageviews},
                FiltersExpression = $"ga:pagePath=~/project/{projectUrlName}/fund"
            };

            var requests = new List<ReportRequest> {tokenPageVisitsRequest, buyPageVisitsRequest};

            var getReport = new GetReportsRequest {ReportRequests = requests};

            var res = await service.Reports.BatchGet(getReport).ExecuteAsync();

            var statisticsExist = res != null && res.Reports != null && res.Reports.Count == 2;

            if (statisticsExist is false)
            {
                return new PageVisitsCounterDto { BuyPageVisits = 0, TokenPageVisits = 0 };
            }
            
            var statisticsResults = ProcessApiResponseToVisitsAmount(res.Reports);
            var tokenPageVisitsAmountResult = statisticsResults[0];
            var buyPageVisitsAmountResult = statisticsResults[1];
            
            return new PageVisitsCounterDto
            {
                TokenPageVisits = tokenPageVisitsAmountResult,
                BuyPageVisits = buyPageVisitsAmountResult
            };
        }
        
        public async 
            Task<PageVisitsDatasetDto> 
            PagesVisitsAsync(string projectUrlName, DateTimeOffset start, DateTimeOffset end)
        {
            var service = await Connect();

            var endDate = end.ToString("yyyy-MM-dd");
            string startDate = start.ToString("yyyy-MM-dd");

            var dateRange = new DateRange {StartDate = startDate, EndDate = endDate};

            var sessions = new Metric {Expression = "ga:uniquePageviews", Alias = "Unique Pageviews"};
            
            var year = new Dimension {Name = "ga:year"};
            var month = new Dimension {Name = "ga:month"};
            var day = new Dimension {Name = "ga:day"};

            var reportTokenPageRequest = new ReportRequest
            {
                ViewId = _googleAnalyticsApiConfig.ReportViewId,
                DateRanges = new List<DateRange> {dateRange},
                Dimensions = new List<Dimension> {year, month, day},
                Metrics = new List<Metric> {sessions},
                FiltersExpression = $"ga:pagePath=~/project/{projectUrlName}/.*"
            };
            
            var reportBuyPageRequest = new ReportRequest
            {
                ViewId = _googleAnalyticsApiConfig.ReportViewId,
                DateRanges = new List<DateRange> {dateRange},
                Dimensions = new List<Dimension> {year, month, day},
                Metrics = new List<Metric> {sessions},
                FiltersExpression = $"ga:pagePath=~/project/{projectUrlName}/fund"
            };

            var requests = new List<ReportRequest> {reportTokenPageRequest, reportBuyPageRequest};

            var getReport = new GetReportsRequest {ReportRequests = requests};

            var analyticsResponse = await service.Reports.BatchGet(getReport).ExecuteAsync();
            
            var tokenPageResult = new List<VisitsDto>();
            var buyPageResult = new List<VisitsDto>();

            if (analyticsResponse?.Reports != null)
            {
                if (analyticsResponse.Reports[0] != null)
                {
                    tokenPageResult = ProcessApiResponseToVisitsCollection(
                        analyticsResponse.Reports[0]
                    );
                    tokenPageResult = FillVisitsEmptyDayValueWithDefaultValue(tokenPageResult, start, end);
                }

                if (analyticsResponse.Reports[1] != null)
                {
                    buyPageResult = ProcessApiResponseToVisitsCollection(
                        analyticsResponse.Reports[1]
                    );
                    buyPageResult = FillVisitsEmptyDayValueWithDefaultValue(buyPageResult, start, end);
                }
            }
            
            return new PageVisitsDatasetDto
            {
                FundPageVisits = buyPageResult,
                TokenPageVisits = tokenPageResult
            };
        }

        private List<int> ProcessApiResponseToVisitsAmount(IList<Report> reports)
        {
            var result = new List<int>();
            foreach (Report report in reports)
            {
                ColumnHeader header = report.ColumnHeader;

                List<MetricHeaderEntry> metricHeaders = (List<MetricHeaderEntry>)header.MetricHeader.MetricHeaderEntries;
                List<ReportRow> rows = (List<ReportRow>)report.Data.Rows;

                if (rows == null)
                {
                    result.Add(0);
                }
                else
                {
                    foreach (ReportRow row in rows)
                    {
                        List<DateRangeValues> metrics = (List<DateRangeValues>)row.Metrics;

                        if (metrics != null)
                        {
                            for (int j = 0; j < metrics.Count; j++)
                            {
                                DateRangeValues values = metrics[j];
                                for (int k = 0; k < values.Values.Count && k < metricHeaders.Count; k++)
                                {
                                    result.Add(Int32.Parse(values.Values[k]));
                                }
                            }   
                        }
                    }
                }
            }

            return result;
        }

        private List<VisitsDto> ProcessApiResponseToVisitsCollection(Report report)
        {
            var result = new List<VisitsDto>();
            VisitsDto entry;
            string dateFormat = @"{0}-{1}-{2}";
            string dateValue;
            int visitsAmount;
            
            List<ReportRow> rows = (List<ReportRow>)report.Data.Rows;

            if (rows == null)
            {
                return result;
            }

            foreach (ReportRow row in rows)
            {
                entry = new VisitsDto();
                
                List<string> dimensions = (List<string>)row.Dimensions;
                List<DateRangeValues> metrics = (List<DateRangeValues>)row.Metrics;
                
                dateValue = string.Format(dateFormat, dimensions.ToArray());
                
                DateRangeValues values = metrics[0];
                visitsAmount = Int16.Parse(values.Values[0]);
                
                entry.Date = dateValue;
                entry.Amount = visitsAmount;
                
                result.Add(entry);
            }
            
            return result;
        }

        private List<VisitsDto> FillVisitsEmptyDayValueWithDefaultValue
            (List<VisitsDto> resultSet, DateTimeOffset start, DateTimeOffset end)
        {
                    
            List<VisitsDto> setWithFullFilledGaps = new List<VisitsDto>();

            int i = 0;
            int buyPageResultLength = resultSet.Count;

            bool dayExistInResultSet = true;
            
            VisitsDto item;
            foreach (DateTime dayFromDateRange in EachDay(start, end))
            {
                item = i < buyPageResultLength ? resultSet[i] : null;

                dayExistInResultSet = 
                    item != null && 
                    String.Equals(item.Date, dayFromDateRange.ToString("yyyy-MM-dd"));
                
                if (dayExistInResultSet is false)
                {
                    setWithFullFilledGaps.Add(new VisitsDto
                    {
                        Amount = 0,
                        Date = dayFromDateRange.ToString("yyyy-MM-dd")
                    });
                    continue;
                }
                
                setWithFullFilledGaps.Add(item);
                i++;
            }
            
            return setWithFullFilledGaps;
        }
        
        private IEnumerable<DateTime> EachDay(DateTimeOffset from, DateTimeOffset to)
        {
            for(var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}