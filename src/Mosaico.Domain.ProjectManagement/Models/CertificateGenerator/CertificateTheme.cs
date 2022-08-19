namespace Mosaico.Domain.ProjectManagement.Models.CertificateGenerator
{
    public class CertificateTheme
    {
        public string BasePath { get; set; }
        public string BackgroundUrl { get; set; }
        public string LogoUrl { get; set; }
        public bool ShowName { get; set; }
        public bool ShowTokensAmount { get; set; }
        public bool ShowDate { get; set; }
        public bool ShowCode { get; set; }
        public bool ShowLogo { get; set; }
        public ElementConfiguration NameConfiguration { get; set; }
        public ElementConfiguration TokensAmountConfiguration { get; set; }
        public ElementConfiguration DateConfiguration { get; set; }
        public ElementConfiguration CodeConfiguration { get; set; }
        public ElementConfiguration LogoConfiguration { get; set; }
    }
}