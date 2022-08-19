namespace Mosaico.Domain.ProjectManagement.Models.CertificateGenerator
{
    public class InvestorCertificateConfiguration
    {
        public string InvestorEmail { get; set; }
        public string TokenTicker { get; set; }
        public string Name { get; set; }
        public string TokensAmount { get; set; }
        public string Date { get; set; }
        public string LogoUrl { get; set; }
        public string CertificateCode { get; set; }
        public string BackgroundUrl { get; set; }
        public string Language { get; set; }
        public CertificateConfiguration CertificateParams { get; set; }
    }
}