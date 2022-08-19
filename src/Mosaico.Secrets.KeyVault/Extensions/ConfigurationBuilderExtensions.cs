using Microsoft.Extensions.Configuration;
using Mosaico.Secrets.KeyVault.Configurations;

namespace Mosaico.Secrets.KeyVault.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddKeyVaultSecrets(this IConfigurationBuilder builder)
        {
            var tempConfig = builder.Build();
            var keyVaultSecretConfig = new KeyVaultConfiguration();
            tempConfig.GetSection(KeyVaultConfiguration.SectionName)?.Bind(keyVaultSecretConfig);
            if (keyVaultSecretConfig.IsEnabled)
            {
                builder.AddAzureKeyVault(keyVaultSecretConfig.Endpoint, keyVaultSecretConfig.ClientId, keyVaultSecretConfig.ClientSecret);
            }

            return builder;
        }
    }
}