using System.Threading.Tasks;
using Mosaico.PDF.Base.Generator.Models;
using Wkhtmltopdf.NetCore;
using Wkhtmltopdf.NetCore.Options;

namespace Mosaico.PDF.Base.Generator
{
    public class PdfGenerator : BasePdfGenerator, IPdfClient
    {
        private readonly IGeneratePdf _generatePdf;
        
        public PdfGenerator(IGeneratePdf generatePdf)
        {
            _generatePdf = generatePdf;
        }
        
        public async Task<byte[]> GenerateAsync(string template, object configuration)
        {
            var certificateContent = await ProcessHtmlTemplate(configuration, template);
            
            var options = new CustomConvertOptions
            {
                PageOrientation = Orientation.Landscape,
                PageMargins = new Margins(0 ,0 ,0 ,0),
                KeepRelative = true
            };
            
            _generatePdf.SetConvertOptions(options);
            
            return _generatePdf.GetPDF(certificateContent);
        }
    }
}