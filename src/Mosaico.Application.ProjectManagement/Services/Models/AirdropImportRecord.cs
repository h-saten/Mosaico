using CsvHelper.Configuration.Attributes;

namespace Mosaico.Application.ProjectManagement.Services.Models
{
    public class AirdropImportRecord
    {
        [Index(0)]
        public string Email { get; set; }
        [Index(1)]
        public decimal Amount { get; set; }
    }
}