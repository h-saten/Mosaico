using System;
using Autofac;
using FluentValidation;
using MediatR.Extensions.Autofac.DependencyInjection;
using Mosaico.Application.Wallet.Abstractions;
using Mosaico.Application.Wallet.BackgroundJobs;
using Mosaico.Application.Wallet.Caching;
using Mosaico.Application.Wallet.Commands.Wallet.WalletCurrencySend;
using Mosaico.Application.Wallet.Commands.Wallet.WalletTokenSend;
using Mosaico.Application.Wallet.DAL;
using Mosaico.Application.Wallet.Permissions;
using Mosaico.Application.Wallet.Services;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Extensions;
using Mosaico.Domain.Wallet;
using Mosaico.Export.Base;
using Mosaico.SDK.Features;
using Mosaico.SDK.ProjectManagement;
using Mosaico.Validation.Base.Extensions;

namespace Mosaico.Application.Wallet
{
    public class WalletApplicationModule : Module
    {
        public static readonly Type MappingProfileType = typeof(WalletMapperProfile);
        
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterMediatR(ThisAssembly);
            builder.RegisterModule(new ProjectManagementSDKModule());
            builder.RegisterModule<WalletDomainModule>();
            builder.RegisterType<BalanceCache>();
            builder.RegisterType<CompanyBalanceCache>();
            builder.RegisterType<TransactionsRepository>().As<ITransactionsRepository>();
            builder.RegisterModule<FeaturesSDKModule>();
            //validators
            builder.RegisterAssemblyValidators(ThisAssembly);
            
            // background jobs
            //builder.RegisterHangfireJob<TransactionsConfirmationJob>();
            builder.RegisterType<WalletSendCurrencyCommandValidator>().As<IValidator<WalletSendCurrencyCommand>>();
            builder.RegisterType<WalletSendTokenCommandValidator>().As<IValidator<WalletSendTokenCommand>>();
            //builder.RegisterType<ScanBlockchainTransactionsJob>().As<IBackgroundJob>();
            builder.RegisterType<FetchExchangeRateJob>().As<IBackgroundJob>();
            //builder.RegisterType<PerformWalletSnapshotJob>().As<IBackgroundJob>();
            builder.RegisterType<EstimateContractDeploymentJob>().As<IBackgroundJob>();
            //builder.RegisterHangfireJob<ScanTokenHoldersJob>();
            builder.RegisterHangfireJob<ConfirmRampTransactionJob>();
            builder.RegisterHangfireJob<ConfirmTransakTransactionJob>();
            builder.RegisterHangfireJob<ConfirmMetamaskTransactionJob>();
            builder.RegisterHangfireJob<FetchKangaMarketsJob>();
            builder.RegisterHangfireJob<ScanPurchaseOperationsJob>();
            builder.RegisterHangfireJob<ExpirePendingTransactionsJob>();
            builder.RegisterHangfireJob<FetchRahimCoinPriceJob>();
            builder.RegisterHangfireJob<ConfirmBinanceTransactionJob>();
            builder.RegisterHangfireJob<ExpireTokenLocksBackgroundJob>();
            //builder.RegisterHangfireJob<ScanProjectInvestorBalancesJob>();
            // services
            builder.RegisterType<UserWalletService>().As<IUserWalletService>();
            builder.RegisterType<DisplayNameFinder>().As<IDisplayNameFinder>();
            builder.RegisterType<WalletValueEstimateService>().As<IWalletValueEstimateService>();
            builder.RegisterType<BuyTokenService>().As<IBuyTokenService>();
            builder.RegisterType<TokenPermissionFactory>().As<ITokenPermissionFactory>();
            builder.RegisterType<TokenomyService>().As<ITokenomyService>();
            builder.RegisterType<CompanyWalletService>().As<ICompanyWalletService>();
            builder.RegisterType<MetamaskDeploymentEstimator>().As<IGasEstimator>();
            builder.RegisterType<TokenHoldersIndexer>().As<ITokenHoldersIndexer>();
            builder.RegisterType<ProjectTransactionsDTOAggregatorService>().As<IProjectTransactionsDTOAggregatorService>();
            builder.RegisterType<CrowdsalePurchaseService>().As<ICrowdsalePurchaseService>();
            builder.RegisterType<ProjectWalletService>().As<IProjectWalletService>();
            builder.RegisterType<ExchangeRateService>().As<IExchangeRateService>();
            builder.RegisterType<BankTransferReferenceService>().As<IBankTransferReferenceService>();
            builder.RegisterType<WalletEmailService>().As<IWalletEmailService>();
            builder.RegisterType<TransactionService>().As<ITransactionService>();
            builder.RegisterType<TokenDistributionWalletService>().As<ITokenDistributionWalletService>();
            builder.RegisterType<FeeService>().As<IFeeService>();
            builder.RegisterType<OperationService>().As<IOperationService>();
            builder.RegisterType<WalletStakingService>().As<IWalletStakingService>();
            builder.RegisterType<TokenLockService>().As<ITokenLockService>();
            //others
            builder.RegisterGeneric(typeof(CsvExporter<>)).As(typeof(IExporter<>));

        }
    }
}