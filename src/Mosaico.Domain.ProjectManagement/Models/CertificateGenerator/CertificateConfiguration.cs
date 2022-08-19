namespace Mosaico.Domain.ProjectManagement.Models.CertificateGenerator
{
    public class CertificateConfiguration
    {
        public BaseTextBlock InvestorName { get; set; }
        public TokensAmountBaseBlock TokensAmount { get; set; }
        public BaseTextBlock Date { get; set; }
        public BaseTextBlock Code { get; set; }
        public LogoBlock LogoBlock { get; set; }
    }
}