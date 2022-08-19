using Microsoft.EntityFrameworkCore;
using Mosaico.Core.EntityFramework;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Staking;

namespace Mosaico.Domain.Wallet.Abstractions
{
    public interface IWalletDbContext : IDbContext
    {
        DbSet<Entities.Wallet> Wallets { get; set; }
        DbSet<CompanyWallet> CompanyWallets { get; set; }
        DbSet<Token> Tokens { get; set; }
        DbSet<Transaction> Transactions { get; set; }
        DbSet<TransactionStatus> TransactionStatuses { get; set; }
        DbSet<TransactionType> TransactionType { get; set; }
        DbSet<TokenType> TokenTypes { get; set; }
        DbSet<PaymentCurrency> PaymentCurrencies { get; set; }
        DbSet<ExternalExchange> ExternalExchanges { get; set; }
        DbSet<TokenToExternalExchange> TokenToExternalExchanges { get; set; }
        DbSet<Vesting> Vestings { get; set; }
        DbSet<Staking> Stakings { get; set; }
        DbSet<VestingFund> VestingFunds { get; set; }
        DbSet<TokenDistribution> TokenDistributions { get; set; }
        DbSet<ExchangeRate> ExchangeRates { get; set; }
        DbSet<TokenHolder> TokenHolders { get; set; }
        DbSet<TokenHolderScanJobLog> TokenHolderScanJobLogs { get; set; }
        DbSet<ProjectWallet> ProjectWallets { get; set; }
        DbSet<ProjectWalletAccount> ProjectWalletAccounts { get; set; }
        DbSet<ProjectBankPaymentDetails> ProjectBankPaymentDetails { get; set; }
        DbSet<ProjectBankTransferTitle> ProjectBankTransferTitles { get; set; }
        DbSet<Deflation> Deflations { get; set; }
        DbSet<NFTCollection> NFTCollections { get; set; }
        DbSet<NFT> NFTs { get; set; }
        DbSet<Investor> Investors { get; set; }
        DbSet<Vault> Vaults { get; set; }
        DbSet<WalletToVesting> WalletToVestings { get; set; }
        DbSet<FeeToProject> FeeToProjects { get; set; }
        DbSet<Operation> Operations { get; set; }
        DbSet<SalesAgent> SalesAgents { get; set; }
        DbSet<StakingPair> StakingPairs { get; set; }
        DbSet<StakingClaimHistory> StakingClaimHistory { get; set; }
        DbSet<TokenLock> TokenLocks { get; set; }
        DbSet<StakingRegulation> StakingRegulations { get; set; }
        DbSet<StakingTerms> StakingTerms { get; set; }
    }
}