using Autofac;
using KangaExchange.SDK;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Mosaico.Application.KangaWallet.Services;

namespace Mosaico.Application.KangaWallet
{
    public class KangaWalletModule : Module
    {
        private readonly IConfiguration _configuration;

        public KangaWalletModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterMediatR(ThisAssembly);
            builder.RegisterModule(new KangaModule(_configuration));
            builder.RegisterType<KangaTransactionRepository>().As<IKangaTransactionRepository>();
        }
    }
}