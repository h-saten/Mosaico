using System.Threading.Tasks;

namespace Mosaico.PDF.Base
{
    public interface IInvestorCertificateGenerator
    {
        Task<byte[]> PdfBytesToImageBytes(byte[] pdfBytes);
        // Task<string> GeneratePdfPath(Action<CertificateGeneratorConfiguration> config);
        // Task<byte[]> PdfToImage(Action<CertificateGeneratorConfiguration> config);
        // Task<byte[]> GeneratePdf(Action<CertificateGeneratorConfiguration> config);
    }
}