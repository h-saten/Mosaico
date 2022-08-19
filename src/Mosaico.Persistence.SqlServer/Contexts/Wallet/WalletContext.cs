using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Mosaico.Core.EntityFramework;
using Mosaico.Core.EntityFramework.Abstractions;
using Mosaico.Domain.Statistics.Entities;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Staking;
using Mosaico.Domain.Wallet.Extensions;

namespace Mosaico.Persistence.SqlServer.Contexts.Wallet
{
    public class WalletContext : DbContextBase<WalletContext>, IWalletDbContext
    {
        public DbSet<Domain.Wallet.Entities.Wallet> Wallets { get; set; }
        public DbSet<CompanyWallet> CompanyWallets { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionStatus> TransactionStatuses { get; set; }
        public DbSet<TransactionType> TransactionType { get; set; }
        public DbSet<TokenType> TokenTypes { get; set; }
        public DbSet<PaymentCurrency> PaymentCurrencies { get; set; }
        public DbSet<ExternalExchange> ExternalExchanges { get; set; }
        public DbSet<TokenToExternalExchange> TokenToExternalExchanges { get; set; }
        public DbSet<Vesting> Vestings { get; set; }
        public DbSet<Staking> Stakings { get; set; }
        public DbSet<VestingFund> VestingFunds { get; set; }
        public DbSet<TokenDistribution> TokenDistributions { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<TokenHolder> TokenHolders { get; set; }
        public DbSet<TokenHolderScanJobLog> TokenHolderScanJobLogs { get; set; }
        public DbSet<ProjectWallet> ProjectWallets { get; set; }
        public DbSet<ProjectWalletAccount> ProjectWalletAccounts { get; set; }
        public DbSet<ProjectBankPaymentDetails> ProjectBankPaymentDetails { get; set; }
        public DbSet<ProjectBankTransferTitle> ProjectBankTransferTitles { get; set; }
        public DbSet<Deflation> Deflations { get; set; }
        public DbSet<NFTCollection> NFTCollections { get; set; }
        public DbSet<NFT> NFTs { get; set; }
        public DbSet<Investor> Investors { get; set; }
        public DbSet<Vault> Vaults { get; set; }
        public DbSet<WalletToVesting> WalletToVestings { get; set; }
        public DbSet<FeeToProject> FeeToProjects { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<SalesAgent> SalesAgents { get; set; }
        public DbSet<StakingPair> StakingPairs { get; set; }
        public DbSet<StakingClaimHistory> StakingClaimHistory { get; set; }
        public DbSet<TokenLock> TokenLocks { get; set; }
        public DbSet<StakingRegulation> StakingRegulations { get; set; }
        public DbSet<StakingTerms> StakingTerms { get; set; }

        public WalletContext(DbContextOptions<WalletContext> options, IEnumerable<ISaveChangesCommandInterceptor> saveChangesCommandInterceptor = null) : base(options, saveChangesCommandInterceptor)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Domain.Wallet.Constants.Schema);
            modelBuilder.ApplyWalletConfiguration();
            base.OnModelCreating(modelBuilder);
        }
        
        public string ContextName => "core";
    }
}