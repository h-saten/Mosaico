using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Wallet.EntityConfigurations;
using Mosaico.Domain.Wallet.EntityConfigurations.Staking;

namespace Mosaico.Domain.Wallet.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyWalletConfiguration(this ModelBuilder builder)
        {
            builder.ApplyConfiguration(new TokenEntityConfiguration());
            builder.ApplyConfiguration(new WalletEntityConfiguration());
            builder.ApplyConfiguration(new WalletToTokenEntityConfiguration());
            builder.ApplyConfiguration(new TransactionEntityConfiguration());
            builder.ApplyConfiguration(new TransactionStatusEntityConfiguration());
            builder.ApplyConfiguration(new TransactionTypeEntityConfiguration());
            builder.ApplyConfiguration(new TokenTypeEntityConfiguration());
            builder.ApplyConfiguration(new PaymentCurrencyConfiguration());
            builder.ApplyConfiguration(new CompanyWalletEntityConfiguration());
            builder.ApplyConfiguration(new CompanyWalletToTokenEntityConfiguration());
            builder.ApplyConfiguration(new ExternalExchangeConfiguration());
            builder.ApplyConfiguration(new TokenToExternalExchangeEntityConfiguration());
            builder.ApplyConfiguration(new TokenToExternalExchangeEntityConfiguration());
            builder.ApplyConfiguration(new VestingEntityConfiguration());
            builder.ApplyConfiguration(new VestingFundEntityConfiguration());
            builder.ApplyConfiguration(new StakingEntityConfiguration());
            builder.ApplyConfiguration(new TokenDistributionEntityConfiguration());
            builder.ApplyConfiguration(new ExchangeRateEntityConfiguration());
            builder.ApplyConfiguration(new ProjectWalletEntityConfiguration());
            builder.ApplyConfiguration(new ProjectWalletAccountEntityConfiguration());
            builder.ApplyConfiguration(new ProjectBankPaymentDetailsEntityConfiguration());
            builder.ApplyConfiguration(new ProjectBankTransferTitleEntityConfiguration());
            builder.ApplyConfiguration(new DeflationEntityConfiguration());
            builder.ApplyConfiguration(new NFTEntityConfiguration());
            builder.ApplyConfiguration(new NFTCollectionEntityConfiguration());
            builder.ApplyConfiguration(new InvestorEntityConfiguration());
            builder.ApplyConfiguration(new VaultEntityConfiguration());
            builder.ApplyConfiguration(new WalletToVestingEntityConfiguration());
            builder.ApplyConfiguration(new OperationEntityConfiguration());
            builder.ApplyConfiguration(new SalesAgentEntityConfiguration());
            builder.ApplyConfiguration(new PaymentCurrencyToStakingPairEntityConfiguration());
            builder.ApplyConfiguration(new StakingPairEntityConfiguration());
            builder.ApplyConfiguration(new StakingClaimHistoryEntityConfiguration());
            builder.ApplyConfiguration(new TokenLockEntityConfiguration());
            builder.ApplyConfiguration(new StakingRegulationEntityConfiguration());
            builder.ApplyConfiguration(new StakingRegulationTranslationEntityConfiguration());
            builder.ApplyConfiguration(new StakingTermsEntityConfiguration());
            builder.ApplyConfiguration(new StakingTermsTranslationEntityConfiguration());
        }
    }
}