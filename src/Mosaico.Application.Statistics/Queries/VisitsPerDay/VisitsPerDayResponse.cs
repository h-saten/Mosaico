using System.Collections.Generic;
using Mosaico.Analytics.Base.Models;

namespace Mosaico.Application.Statistics.Queries.VisitsPerDay
{
    public class VisitsPerDayResponse
    {
        public List<VisitsDto> TokenPageVisits { get; set; }
        public List<VisitsDto> FundPageVisits { get; set; }
    }
}