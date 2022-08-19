using System.Collections.Generic;

namespace Mosaico.Analytics.Base.Models
{
    public class PageVisitsDatasetDto
    {
        public List<VisitsDto> TokenPageVisits { get; set; }
        public List<VisitsDto> FundPageVisits { get; set; }
    }
}