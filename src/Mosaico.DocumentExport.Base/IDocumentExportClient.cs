using System.Threading.Tasks;

namespace Mosaico.DocumentExport.Base
{
    public interface IDocumentExportClient
    {
        public Task<byte[]> ExportAsync(string html);
    }
}