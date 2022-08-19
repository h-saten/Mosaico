namespace Mosaico.Statistics.GoogleAnalytics.Configuration
{
    public class GoogleAnalyticsConfiguration
    {
        public const string SectionName = "GoogleAnalytics";
        
        public string AccountEmail { get; set; }
        public string KeyVaultCertificateName { get; set; }
        public string ApplicationName { get; set; }
        public string ReportViewId { get; set; }
    }
}