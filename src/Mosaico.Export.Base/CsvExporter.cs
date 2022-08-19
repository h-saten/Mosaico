using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace Mosaico.Export.Base
{
    public class CsvExporter<TEntity> : IExporter<TEntity> where TEntity : class
    {
        public virtual async Task<byte[]> ExportAsync(List<TEntity> entities)
        {
            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                HasHeaderRecord = true
            };
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    using (var csv = new CsvWriter(streamWriter, csvConfiguration))
                    {
                        csv.WriteHeader<TEntity>();
                        await csv.NextRecordAsync();
                        await csv.WriteRecordsAsync(entities);
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }
}