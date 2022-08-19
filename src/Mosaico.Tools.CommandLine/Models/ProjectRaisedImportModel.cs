using CsvHelper.Configuration.Attributes;

namespace Mosaico.Tools.CommandLine.Models
{
    public class ProjectRaisedImportModel
    {
        [Index(0)]
        public string ProjectCode { get; set; }
        [Index(1)]
        public decimal? Raised { get; set; }
        [Index(2)]
        public bool Staking { get; set; }
        [Index(3)]
        public bool Vesting { get; set; }
        [Index(4)]
        public bool Deflation { get; set; }
    }
}