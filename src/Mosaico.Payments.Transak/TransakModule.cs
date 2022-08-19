using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Payments.Transak.Abstractions;
using Mosaico.Payments.Transak.Configurations;

namespace Mosaico.Payments.Transak
{
    public class TransakModule : Module
    {
        private readonly IConfiguration _configuration;

        public TransakModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<TransakClient>().As<ITransakClient>();
            var settings = new TransakConfiguration();
            _configuration.GetSection(TransakConfiguration.SectionName).Bind(settings);
            builder.RegisterInstance(settings);
        }
    }
}