using System;
using System.Collections.Generic;
using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mosaico.API.v1.Wallet;
using Mosaico.Application.ProjectManagement;
using Mosaico.Application.Wallet;
using Mosaico.Base.Extensions;
using Mosaico.Base.Settings;
using Mosaico.CommandLine.Base;
using Mosaico.Core;
using Mosaico.Core.EntityFramework.Configurations;
using Mosaico.Core.ResourceManager;
using Mosaico.Domain.Statistics;
using Mosaico.Integration.Blockchain.CoinAPI;
using Mosaico.Integration.Blockchain.Ethereum;
using Mosaico.Integration.Blockchain.Moralis;
using Mosaico.Integration.Email.SendGridEmail;
using Mosaico.Payments.Binance;
using Mosaico.Persistence.SqlServer;
using Mosaico.SDK.Identity;
using Mosaico.SDK.Relay;
using Mosaico.SDK.Wallet;
using Mosaico.Tools.CommandLine.Commands;
using Serilog;

namespace Mosaico.Tools.CommandLine
{
    public class CommandLineModule : Module
    {
        private readonly IConfiguration _configuration;

        public CommandLineModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            
            // required for db context to be registered
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            builder.RegisterInstance(serviceProvider).AsImplementedInterfaces().SingleInstance();
            
            var loggersSettings = new LoggersSetting();
            _configuration.GetSection(LoggersSetting.SectionName).Bind(loggersSettings);
            var logger = new LoggerConfiguration()
                .UseConsoleLogger(loggersSettings.ConsoleLogger)
                .CreateLogger();
            Log.Logger = logger;
            builder.RegisterInstance(logger).As<ILogger>();

            //modules
            var sqlServerSettings = new SqlServerConfiguration();
            _configuration.GetSection(SqlServerConfiguration.SectionName).Bind(sqlServerSettings);
            
            var mappingTypes = WalletAPIv1Module.MappingProfileType;
            mappingTypes.Add(WalletSDKModule.MappingSDKProfileType);
            builder.RegisterModule<ResourceManagerModule>();
            builder.RegisterModule(new CoreModule(mappingTypes, _configuration));
            builder.RegisterModule(new DomainModule(_configuration));
            builder.RegisterModule(new IdentityDomainModule(_configuration, Domain.Identity.Constants.IdentityServerDbConfigurationSection));
            builder.RegisterModule(new CoinApiModule(_configuration));
            builder.RegisterModule(new WalletSDKModule());
            builder.RegisterModule(new StatisticsModule());
            builder.RegisterModule(new WalletApplicationModule());
            builder.RegisterModule(new ProjectManagementApplicationModule());
            builder.RegisterModule(new MoralisModule(_configuration));
            builder.RegisterModule(new SendGridEmailModule(_configuration));
            builder.RegisterModule(new BinanceModule(_configuration));
            builder.RegisterModule(new RelayModule(_configuration));
            
            builder.AddUrl("https://mosaico.ai", "BaseUri");
            
            //commands
            builder.RegisterType<TestCommand>().As<ICommand>();
            builder.RegisterType<GenerateFakeDataCommand>().As<ICommand>();
            builder.RegisterType<GenerateFakePaymentCurrencyDataCommand>().As<ICommand>();
            builder.RegisterType<TopUpWalletByUsdtCommand>().As<ICommand>();
            builder.RegisterType<MigrateDatabaseCommand>().As<ICommand>();
            builder.RegisterType<ImportPaymentCurrencyCommand>().As<ICommand>();
            builder.RegisterType<TestMetaTransactionsCommand>().As<ICommand>();
            builder.RegisterType<GetPrivateKeyCommand>().As<ICommand>();
            builder.RegisterType<WithdrawTokenCommand>().As<ICommand>();
            builder.RegisterType<ExportProjectWalletCommand>().As<ICommand>();
            builder.RegisterType<RecalculateIncomeCommand>().As<ICommand>();
            builder.RegisterType<ExportInvestorInformationCommand>().As<ICommand>();
            builder.RegisterType<ValidateTransactionsCommand>().As<ICommand>();
            builder.RegisterType<MigrateLikesFromInvestorsCommand>().As<ICommand>();
            builder.RegisterType<RecalculateFeeCommand>().As<ICommand>();
            builder.RegisterType<AcceptTransactionsCommand>().As<ICommand>();
            builder.RegisterType<ImportStakingPairsCommand>().As<ICommand>();
            builder.RegisterType<ImportTokensCommand>().As<ICommand>();
            builder.RegisterType<SendAffiliationProgramInvitationsCommand>().As<ICommand>();
            builder.RegisterType<AssignStakingCommand>().As<ICommand>();
            builder.RegisterType<ImportFundCommand>().As<ICommand>();
            builder.RegisterType<PayDividendCommand>().As<ICommand>();
            builder.RegisterType<ImportStakingCommand>().As<ICommand>();
            builder.RegisterType<DistributeMosaicoStakingCommand>().As<ICommand>();
            builder.RegisterType<CleanStakingCommand>().As<ICommand>();
            builder.RegisterType<ImportTransakTransactionCommand>().As<ICommand>();
        }
    }
}