using System.Threading.Tasks;
using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Mosaico.Base.Abstractions;
using Mosaico.Statistics.GoogleAnalytics.Abstractions;
using Mosaico.Statistics.GoogleAnalytics.Configuration;

namespace Mosaico.Statistics.GoogleAnalytics
{
    public class GoogleAnalyticsConnector : IAnalyticsReportingConnector
    {
        private readonly GoogleAnalyticsConfiguration _googleAnalyticsApiConfig;
        private readonly ICertificateService _certificateService;

        public GoogleAnalyticsConnector(GoogleAnalyticsConfiguration googleAnalyticsApiConfig, ICertificateService certificateService)
        {
            _googleAnalyticsApiConfig = googleAnalyticsApiConfig;
            _certificateService = certificateService;
        }

        public async Task<AnalyticsReportingService> Connect()
        {
            string[] scopes = {AnalyticsReportingService.Scope.AnalyticsReadonly};
            
            // found https://console.developers.google.com
            var serviceAccountEmail = _googleAnalyticsApiConfig.AccountEmail;

            var certificate = await _certificateService.GetCertificateAsync(_googleAnalyticsApiConfig.KeyVaultCertificateName);
            
            var credential = new ServiceAccountCredential( 
                new ServiceAccountCredential.Initializer(serviceAccountEmail) {
                    Scopes = scopes
                }.FromCertificate(certificate));
            
            return new AnalyticsReportingService(new BaseClientService.Initializer { 
                HttpClientInitializer = credential,
                ApplicationName = _googleAnalyticsApiConfig.ApplicationName
            });
        }
    }
}