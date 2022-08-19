namespace Mosaico.Identity.Models
{
    public class AppConfigurationDTO
    {
        public string MainUrl { get; set; }
        public string KangaAppId { get; set; }
        public string KangaBaseUrl { get; set; }
        public string AfterLogoutUrl { get; set; }
        public bool GoogleAuthenticationEnabled { get; set; }
        public bool FacebookAuthenticationEnabled { get; set; }
        public bool KangaAuthenticationEnabled { get; set; }
        public string RecaptchaSiteKey { get; set; }
        public string GatewayUrl { get; set; }
    }
}