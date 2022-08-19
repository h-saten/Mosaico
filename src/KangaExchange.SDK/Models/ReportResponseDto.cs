using System.Collections.Generic;

namespace KangaExchange.SDK.Models
{
    public class ReportResponseDto
    {
        public string Result { get; set; }
        public List<ReportEntryDto> Report { get; set; }
        public string Code { get; set; }
    }
}