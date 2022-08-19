using Autofac;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Mosaico.Storage.AzureBlobStorage.Configurations;
using Mosaico.Storage.AzureBlobStorage.Validators;
using Mosaico.Storage.Base;

namespace Mosaico.Storage.AzureBlobStorage
{
    public class AzureBlobStorageModule : Module
    {
        private readonly AzureBlobStorageConfiguration _configuration;

        public AzureBlobStorageModule(IConfiguration config)
        {
            _configuration = new AzureBlobStorageConfiguration();
            config.GetSection(AzureBlobStorageConfiguration.SectionName).Bind(_configuration);
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterInstance(_configuration);
            builder.RegisterType<AzureBlobStorageConfigValidator>().As<IValidator<AzureBlobStorageConfiguration>>();
            builder.RegisterType<AzureBlobStorageClient>().As<IStorageClient>();
        }
    }
}