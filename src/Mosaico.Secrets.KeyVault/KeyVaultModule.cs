using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Base.Abstractions;
using Mosaico.Secrets.KeyVault.Certificates;
using Mosaico.Secrets.KeyVault.Configurations;

namespace Mosaico.Secrets.KeyVault
{
    /*
     * Module which contains registrations of Email API - EmailLabs integration
     */
    public class KeyVaultModule : Module
    {
        private readonly KeyVaultConfiguration _config = new();

        public KeyVaultModule(IConfiguration configuration)
        {
            configuration.GetSection(KeyVaultConfiguration.SectionName).Bind(_config);
        }
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterInstance(_config).AsSelf();
            builder.RegisterType<KeyVaultCertificateService>().As<ICertificateService>();
        }
    }
}