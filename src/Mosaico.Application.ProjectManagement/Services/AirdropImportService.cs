using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Mosaico.Application.ProjectManagement.Abstractions;
using Mosaico.Application.ProjectManagement.Services.Models;

namespace Mosaico.Application.ProjectManagement.Services
{
    public class AirdropImportService : IAirdropImportService
    {
        public async Task<List<AirdropImportRecord>> GetAirdropParticipantsAsync(byte[] file)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };
            using (var memoryStream = new MemoryStream(file))
            {
                using (var reader = new StreamReader(memoryStream))
                {
                    using (var csv = new CsvReader(reader, config))
                    {
                        var records = csv.GetRecords<AirdropImportRecord>();
                        return records.ToList();
                    }
                }
            }
        }
    }
}