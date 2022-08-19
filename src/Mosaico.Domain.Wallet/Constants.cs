using System.Collections.Generic;

namespace Mosaico.Domain.Wallet
{
    public static class Constants
    {
        public const int StandardFee = 7;
        public static class Tables
        {
            public const string Tokens = "Tokens";
            public const string TokenStatuses = "TokenStatuses";
            public const string TokenTypes = "TokenTypes";
            public const string Wallets = "Wallets";
            public const string CompanyWallets = "CompanyWallets";
            public const string WalletToToken = "WalletToToken";
            public const string CompanyWalletToToken = "CompanyWalletToToken";
            public const string WalletToVesting = "WalletToVesting";
            public const string Transactions = "Transactions";
            public const string TransactionStatuses = "TransactionStatuses";
            public const string TransactionTypes = "TransactionTypes";
            public const string VestingWallet = "VestingWallets";
            public const string PaymentCurrency = "PaymentCurrencies";
            public const string CurrencyContractAddress = "CurrencyContractAddresses";
            public const string ExternalExchanges = "ExternalExchanges";
            public const string TokenToExternalExchanges = "TokenToExternalExchanges";
            public const string Vestings = "Vestings";
            public const string Stakings = "Stakings";
            public const string VestingFunds = "VestingFunds";
            public const string VestingInvitations = "VestingInvitations";
            public const string TokenDistributions = "TokenDistributions";
            public const string TokenDistributionWallets = "TokenDistributionWallets";
            public const string ExchangeRates = "ExchangeRates";
            public const string TokenHolders = "TokenHolders";
            public const string TokenHolderScanJobLogs = "TokenHolderScanJobLogs";
            public const string ProjectWallets = "ProjectWallets";
            public const string ProjectWalletAccounts = "ProjectWalletAccounts";
            public const string ProjectBankPaymentDetails = "ProjectBankPaymentDetails";
            public const string ProjectBankTransferTitles = "ProjectBankTransferTitles";
            public const string Deflations = "Deflations";
            public const string NFTs = "NFTs";
            public const string NFTCollections = "NFTCollections";
            public const string Investors = "Investors";
            public const string Vaults = "Vaults";
            public const string FeeToProjects = "FeeToProjects";
            public const string Operations = "Operations";
            public const string SalesAgents = "SalesAgents";
            public const string PaymentCurrencyToStakingPairs = "PaymentCurrencyToStakingPairs";
            public const string StakingPairs = "StakingPairs";
            public const string StakingClaimHistory = "StakingClaimHistory";
            public const string TokenLocks = "TokenLocks";
            public const string StakingRegulations = "StakingRegulations";
            public const string StakingRegulationTranslations = "StakingRegulationTranslations";
            public const string StakingTerms = "StakingTerms";
            public const string StakingTermsTranslations = "StakingTermsTranslations";
        }

        public static class VentureFundTables
        {
            public const string VentureFunds = "VentureFunds";
            public const string VentureFundTokens = "VentureFundTokens";
        }
        
        public const string Schema = "wlt";
        public const string FundSchema = "fund";

        public static class ErrorCodes
        {
            public const string TokenNotFound = "TOKEN_NOT_FOUND";
            public const string TokenTypeNotFound = "TOKEN_TYPE_NOT_FOUND";
            public const string VestingWalletNotFound = "VESTING_WALLET_NOT_FOUND";
            public const string CurrencyNetworkDuplication = "CURRENCY_NETWORK_DUPLICATION";
            public const string TokenStatusNotFound = "TOKEN_STATUS_NOT_FOUND";
            public const string ExternalExchangeNotFound = "EXTERNAL_EXCHANGE_NOT_FOUND";
            public const string InvalidContractAddress = "INVALID_CONTRACT_ADDRESS";
            public const string KPINotFound = "KPI_NOT_FOUND";
        }
        
        public static class TransactionStatuses
        {
            public const string Confirmed = "CONFIRMED";
            public const string Pending = "PENDING";
            public const string Canceled = "CANCELED";
            public const string Failed = "FAILED";
            public const string InProgress = "IN_PROGRESS";
            public const string Refunded = "REFUNDED";
            public const string Expired = "EXPIRED";
        }

        public static class TransactionType
        {
            public const string Purchase = "PURCHASE";
            public const string Deposit = "DEPOSIT";
            public const string Withdrawal = "WITHDRAWAL";
            public const string Exchange = "EXCHANGE";
            public const string Transfer = "TRANSFER";
        }

        public static class TokenType
        {
            public static List<string> All = new List<string> {Utility, /*NFT, Security*/};
            public const string Utility = "UTILITY";
            public const string NFT = "NFT";
            public const string Security = "SECURITY";
        }

        public static class PaymentCurrencies
        {
            public const string USDT = "USDT";
        }
        
        public static class RedisKeys
        {
            public const string Estimates = "estimates";
        }

        public static class DeploymentPaymentMethods
        {
            public static readonly List<string> All = new List<string> {Mosaico, Metamask};
            public const string Mosaico = "MOSAICO_WALLET";
            public const string Metamask = "METAMASK";
        }
    }
}