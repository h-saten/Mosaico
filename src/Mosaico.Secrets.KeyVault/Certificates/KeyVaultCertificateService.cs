using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using FluentValidation;
using Mosaico.Base.Abstractions;
using Mosaico.Base.Exceptions;
using Mosaico.Secrets.KeyVault.Configurations;
using Mosaico.Secrets.KeyVault.Exceptions;
using Mosaico.Secrets.KeyVault.Validators;

namespace Mosaico.Secrets.KeyVault.Certificates
{
    public class KeyVaultCertificateService : ICertificateService
    {
        private readonly KeyVaultConfiguration _configuration;

        public KeyVaultCertificateService(KeyVaultConfiguration configuration)
        {
            _configuration = configuration;
        }

        private TokenCredential GetAzureCredentials()
        {
            return new ClientSecretCredential(_configuration.TenantId, _configuration.ClientId, _configuration.ClientSecret);
        }

        private void ValidateConfiguration()
        {
            var validator = new KeyVaultCertificateConfigurationValidator();
            var result = validator.Validate(_configuration);
            if (!result.IsValid)
            {
                throw new InvalidConfigException(nameof(KeyVaultConfiguration));
            }
        }

        public async Task<X509Certificate2> GetCertificateAsync(string name, string password = null)
        {
            ValidateConfiguration();
            var credential = GetAzureCredentials();
            var secretClient = new SecretClient(vaultUri: new Uri(_configuration.Endpoint), credential);
            var secret = await secretClient.GetSecretAsync(name);
            if (secret?.Value == null)
            {
                throw new SecretNotFoundException(name);
            }
            var privateKeyBytes = Convert.FromBase64String(secret.Value.Value);
            var certificateWithPrivateKey = new X509Certificate2(privateKeyBytes, password, X509KeyStorageFlags.MachineKeySet);
            return certificateWithPrivateKey;
        }
        
        private Task<List<CertificateProperties>> GetCertificateVersionsAsync(string name)
        {
            ValidateConfiguration();
            var credential = GetAzureCredentials();
            var client = new CertificateClient(vaultUri: new Uri(_configuration.Endpoint), credential);
            var certificateVersions = client.GetPropertiesOfCertificateVersions(name);
            return Task.FromResult(certificateVersions
                .Where(certVersion => certVersion.Enabled.HasValue && certVersion.Enabled.Value)
                .OrderByDescending(certVersion => certVersion.CreatedOn)
                .ToList());
        }
    }
}