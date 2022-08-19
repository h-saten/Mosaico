using Newtonsoft.Json;

namespace Mosaico.Secrets.KeyVault.Configurations
{
    public class KeyVaultConfiguration
    {
        public const string SectionName = "KeyVault";
        public bool IsEnabled { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Endpoint { get; set; }
        public string TenantId { get; set; }
    }
}