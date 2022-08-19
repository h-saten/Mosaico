using System;
using System.Collections.Generic;
using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Application.KangaWallet;
using Mosaico.Application.Wallet;
using Mosaico.Integration.Blockchain.Moralis;
using Mosaico.SDK.Wallet;

namespace Mosaico.API.v1.Wallet
{
    public class WalletAPIv1Module : Module
    {
        private readonly IConfiguration _configuration;
        public static readonly List<Type> MappingProfileType = new()
        {
            WalletApplicationModule.MappingProfileType,
            WalletSDKModule.MappingSDKProfileType
        };

        public WalletAPIv1Module(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule<WalletApplicationModule>();
            builder.RegisterModule(new WalletEventModule(_configuration));
            builder.RegisterModule(new MoralisModule(_configuration));
            builder.RegisterModule(new KangaWalletModule(_configuration));
        }
    }
}