using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Docnet.Core;
using Docnet.Core.Models;
using Stubble.Core.Builders;

namespace Mosaico.PDF.Base.Generator
{
    public abstract class BasePdfGenerator
    {
        protected async Task<string> ProcessTemplate(object mustacheModel, string templateUri)
        {
            var stubble = new StubbleBuilder().Build();
            using StreamReader streamReader = new StreamReader(templateUri, Encoding.UTF8);
            var content = await streamReader.ReadToEndAsync();
            var output = await stubble.RenderAsync(content, mustacheModel);

            return output;
        }
        
        protected async Task<string> ProcessHtmlTemplate(object mustacheModel, string template)
        {
            var stubble = new StubbleBuilder().Build();
            var output = await stubble.RenderAsync(template, mustacheModel);

            return output;
        }

        protected string PdfTemporaryPath()
        {
            var tempPath = Path.GetTempPath();
            var fileName = $"{tempPath}{Guid.NewGuid()}";
            return $"{fileName}.pdf";
        }
        
        protected string ImageTemporaryPath()
        {
            var tempPath = Path.GetTempPath();
            var fileName = $"{tempPath}{Guid.NewGuid()}";
            return $"{fileName}.jpeg";
        }

        protected async Task<byte[]> PdfToImageAsBytes(string filePath, int sizeX = 1080, int sizeY = 1920)
        {
            var docLib = DocLib.Instance;

            using var docReader = docLib.GetDocReader(filePath, new PageDimensions(sizeX, sizeY));

            using var pageReader = docReader.GetPageReader(0);

            var rawBytes = pageReader.GetImage();

            var width = pageReader.GetPageWidth();
            var height = pageReader.GetPageHeight();
            
            using var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            AddBytes(bmp, rawBytes);

            await using var stream = new MemoryStream();
            bmp.Save(stream, ImageFormat.Jpeg);

            return stream.ToArray();
        }
        
        protected static void AddBytes(Bitmap bmp, byte[] rawBytes)
        {
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

            var bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
            var pNative = bmpData.Scan0;

            Marshal.Copy(rawBytes, 0, pNative, rawBytes.Length);
            bmp.UnlockBits(bmpData);
        }
    }
}