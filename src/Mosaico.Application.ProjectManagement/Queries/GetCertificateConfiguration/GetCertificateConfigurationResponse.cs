using Mosaico.Domain.ProjectManagement.Models.CertificateGenerator;

namespace Mosaico.Application.ProjectManagement.Queries.GetCertificateConfiguration
{
    public class GetCertificateConfigurationResponse
    {
        public CertificateConfiguration Configuration { get; set; }
        public bool SendCertificateToInvestor { get; set; }
        public bool HasConfiguration { get; set; }
        public bool HasBlocksConfiguration { get; set; }
        public string BackgroundUrl { get; set; }
    }
}