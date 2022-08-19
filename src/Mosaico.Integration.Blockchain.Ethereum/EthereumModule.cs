using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Configuration;
using Mosaico.Integration.Blockchain.Ethereum.DAL;
using Mosaico.Integration.Blockchain.Ethereum.DataSeed;
using Mosaico.Integration.Blockchain.Ethereum.PaymentTokens;
using Mosaico.Integration.Blockchain.Ethereum.Services.v1;
using Mosaico.Integration.Blockchain.Ethereum.Tools;

namespace Mosaico.Integration.Blockchain.Ethereum
{
    public class EthereumModule : Module
    {
        private readonly IConfiguration _configuration;

        public EthereumModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var config = new BlockchainConfiguration();
            _configuration.GetSection(BlockchainConfiguration.SectionName).Bind(config);
            builder.RegisterInstance(config);
            foreach (var networkConfig in config.Networks)
            {
                builder.RegisterInstance(networkConfig).Keyed<EthereumNetworkConfiguration>(networkConfig.Name);
                if (networkConfig.AdminAccountProviderType == Constants.EthereumAdminAccountProviderTypes.Configuration)
                {
                    builder.RegisterType<EthereumAdminAccountConfigurationProvider>().Keyed<IAdminAccountProvider<EthereumAdminAccount>>(networkConfig.Name);
                }
                builder.RegisterType<EthereumClient>().Keyed<IEthereumClient>(networkConfig.Name);
                builder.RegisterType<EthereumServiceFactory>().Keyed<IEthereumServiceFactory>(networkConfig.Name);
            }
            
            // Contracts interaction facades
            builder.RegisterType<StakingService>().As<IStakingService>().Keyed<IStakingService>(Constants.StakingContractVersions.Version1);
            builder.RegisterType<CrowdsaleV1Service>().As<ICrowdsaleService>().Keyed<ICrowdsaleService>(Constants.CrowdsaleContractVersions.Version1);
            builder.RegisterType<TokenService>().As<ITokenService>().Keyed<ITokenService>(Constants.TokenContractVersions.Version1);
            builder.RegisterType<Daov1Service>().As<IDaoService>().Keyed<IDaoService>(Constants.DAOContractVersions.Version1);
            builder.RegisterType<TetherPaymentTokenService>().As<IPaymentTokenService>().Keyed<IPaymentTokenService>(Constants.PaymentCurrencies.USDT);
            builder.RegisterType<Vaultv1Service>().As<IVaultv1Service>();
            
            builder.RegisterType<LocalContractRepository>().As<ICrowdSaleRepository>();
            builder.RegisterType<EtherScanner>().As<IEtherScanner>();
            builder.RegisterType<EthereumClientFactory>().As<IEthereumClientFactory>();
            builder.RegisterType<BlockWithTransactionRepository>().As<IBlockWithTransactionRepository>();
            builder.RegisterType<TokenRepository>().As<ITokenRepository>();
            builder.RegisterType<TransactionRepository>().As<ITransactionRepository>();
            builder.RegisterType<LocalPaymentTokensDeployer>().As<ILocalPaymentTokensDeployer>();
            builder.RegisterType<EtherScanner>().As<IEtherScanner>();
            builder.RegisterType<ContractAnalyzer>().As<IContractAnalyzer>();
        }
    }
}