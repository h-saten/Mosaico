using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Docnet.Core;
using Docnet.Core.Models;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Mosaico.DocumentExport.PDF.Generator
{
    public class PdfGenerator : BasePdfGenerator, IPdfClient
    {
        public async Task<byte[]> HtmlToPdfAsync(string html, bool landscapeOrientation = false, CancellationToken token = new())
        {
            await new BrowserFetcher().DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            await using var page = await browser.NewPageAsync();
            await page.EmulateMediaTypeAsync(MediaType.Screen);
            await page.SetContentAsync(html);
            var pdfContent = await page.PdfStreamAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                Landscape = landscapeOrientation
            });
            await using MemoryStream ms = new MemoryStream();
            await pdfContent.CopyToAsync(ms, token);
            return ms.ToArray();
        }

        public async Task<string> CompileHtmlAsync(string templateContent, object parameters, CancellationToken token = new())
        {
            return await ProcessHtmlTemplate(parameters, templateContent);
        }

        public async Task<PdfImageFile> PdfToImageAsync(byte[] pdfFile, CancellationToken token = new())
        {
            var certificatePdfTemporaryPath = PdfTemporaryPath();
            
            await File.WriteAllBytesAsync(certificatePdfTemporaryPath, pdfFile, token);

            var fileBytes = await PdfToImageAsBytes(certificatePdfTemporaryPath);

            return new PdfImageFile
            {
                ContentType = "image/jpeg",
                FileContent = fileBytes,
            };
        }
    }
}