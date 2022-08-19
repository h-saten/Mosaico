using System.Threading;
using System.Threading.Tasks;
using Mosaico.DocumentExport.PDF.Generator;

namespace Mosaico.DocumentExport.PDF
{
    public interface IPdfClient
    {
        Task<byte[]> HtmlToPdfAsync(string content, bool landscapeOrientation = false, CancellationToken token = new());
        Task<string> CompileHtmlAsync(string templateContent, object parameters, CancellationToken token = new());
        Task<PdfImageFile> PdfToImageAsync(byte[] pdfFile, CancellationToken token = new());
    }
}