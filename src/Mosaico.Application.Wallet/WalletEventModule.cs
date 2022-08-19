using Autofac;
using Microsoft.Extensions.Configuration;
using Mosaico.Application.Wallet.DAL;
using Mosaico.Application.Wallet.EventHandlers;
using Mosaico.Application.Wallet.EventHandlers.Staking;
using Mosaico.Application.Wallet.EventHandlers.Vault;
using Mosaico.Events.Base;

namespace Mosaico.Application.Wallet
{
    public class WalletEventModule : Module
    {
        private readonly IConfiguration _configuration;

        public WalletEventModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<CreateWalletOnUserCreated>().As<IEventHandler>();
            //builder.RegisterType<TransferOnOrderCreated>().As<IEventHandler>();
            builder.RegisterType<TransactionsRepository>().As<ITransactionsRepository>();
            builder.RegisterType<UpdateTokenLogoOnFileUploaded>().As<IEventHandler>();
            builder.RegisterType<PerformNonCustodialTransferOnInitiated>().As<IEventHandler>();
            builder.RegisterType<DeployCompanyOnCreated>().As<IEventHandler>();
            builder.RegisterType<DistributeAirdropOnRequested>().As<IEventHandler>();
            builder.RegisterType<SendBankTransferDetailsOnCreated>().As<IEventHandler>();
            builder.RegisterType<SendEmailOnSuccessfulPurchase>().As<IEventHandler>();
            builder.RegisterType<PerformTransferOnMosaicoCheckoutInitiated>().As<IEventHandler>();
            builder.RegisterType<CreateVaultOnRequested>().As<IEventHandler>();
            builder.RegisterType<CreateVaultDepositOnRequested>().As<IEventHandler>();
            builder.RegisterType<SendVaultTokensOnRequested>().As<IEventHandler>();
            builder.RegisterType<DeployVestingWalletOnRequested>().As<IEventHandler>();
            builder.RegisterType<StakeOnRequested>().As<IEventHandler>();
            builder.RegisterType<WithdrawStakeOnRequested>().As<IEventHandler>();
            builder.RegisterType<ClaimRewardOnRequested>().As<IEventHandler>();
            builder.RegisterType<DistributeStakeOnRequested>().As<IEventHandler>();
            builder.RegisterType<LockTokensOnPurchase>().As<IEventHandler>();
            builder.RegisterType<VerifySoftCapOnPurchase>().As<IEventHandler>();
            builder.RegisterType<AwaitMetamaskStakeOnInitiated>().As<IEventHandler>();
            builder.RegisterType<AwaitMetamaskWithdrawOnInitiated>().As<IEventHandler>();
            builder.RegisterType<UpdateTermsUrlOnUploaded>().As<IEventHandler>();
        }
    }
}