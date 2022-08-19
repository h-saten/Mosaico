using System.Collections.Generic;

namespace Mosaico.Application.Wallet
{
    public static class Constants
    {
        public static class ExportFormats
        {
            public static List<string> All = new List<string> {CSV};
            public const string CSV = "CSV";
        }
        
        public static class FIATCurrencies
        {
            public static List<string> All = new List<string> {USD, EUR, PLN, GBP};
            
            public const string USD = "USD";
            public const string EUR = "EUR";
            public const string PLN = "PLN";
            public const string GBP = "GBP";
        }

        public static class CryptoCurrencies
        {
            public static List<string> All = new() {ETH, BTC, USDC, USDT, MATIC, oPLN, BUSD};
            
            public const string ETH = "ETH";
            public const string BTC = "BTC";
            public const string USDC = "USDC";
            public const string USDT = "USDT";
            public const string MATIC = "MATIC";
            public const string oPLN = "oPLN";
            public const string BUSD = "BUSD";
        }
        public static class ErrorCodes
        {
            public const string InvalidWalletOperation = "INVALID_WALLET_OPERATION";
            public const string InvalidTransactionType = "INVALID_TRANSACTION_TYPE";
            public const string InvalidTransactionStatus = "INVALID_TRANSACTION_STATUS";
            public const string InvalidTokenAmount = "INVALID_TOKEN_AMOUNT";
            public const string UnauthorizedPurchase = "INVALID_PURCHASE";
            public const string ProjectSaleStageNotExist = "PROJECT_SALE_STAGE_NOT_EXIST";
            public const string TransactionStatusNotExist = "TRANSACTION_STATUS_NOT_EXIST";
            public const string UnsupportedCurrency = "UNSUPPORTED_CURRENCY";
            public const string InsufficientFunds = "INSUFFICIENT_FUNDS";
            public const string TokenDistributionAlreadyExists = "TOKEN_DISTRIBUTION_ALREADY_EXISTS";
            public const string TokenDistributionNotFound = "TOKEN_DISTRIBUTION_NOT_FOUND";
            public const string InvalidTokenomy = "INVALID_TOKENOMY";
            public const string TokenNotMintable = "TOKEN_NOT_MINTABLE";
            public const string TokenNotBurnable = "TOKEN_NOT_BURNABLE";
            public const string TokenAlreadyDeployed = "TOKEN_ALREADY_DEPLOYED";
            public const string UnauthorizedCompanyDataAccess = "UNAUTHORIZED_COMPANY_DATA_ACCESS";
        }
        
        public static class PaymentProcessors
        {
            public static readonly List<string> All = new() {Mosaico, Transak, Paypal, Metamask};
            public const string Mosaico = "mosaico";
            public const string Transak = "transak";
            public const string Paypal = "paypal";
            public const string Metamask = "metamask";
        }

        public static class LockReasons
        {
            public const string TRANSFER = "TRANSFER";
            public const string PURCHASE = "PURCHASE";
            public const string STAKE = "STAKE";
        }
        
        public static class Jobs
        {
            public const string TransactionsConfirmationJob = "transactions-confirmation-job";
            public const string ScanBlockchainTransactionsJob = "scan-blockchain-transactions-job";
            public const string ScanProjectWalletBalanceJob = "scan-project-wallet-balance-job";
            public const string FetchExchangeRatesJob = "fetch-exchange-rate-job";
            public const string EstimateGasJob = "estimate-gas-job";
            public const string PerformWalletSnapshotJob = "wallet-snapshot-job";
            public const string ScanTokenHoldersJob = "index-token-holders-job";
            public const string RampTransactionConfirmationJob = "ramp-confirmation-job";
            public const string TransakConfirmationJob = "transak-confirmation-job";
            public const string ExpireTransactionsJob = "expire-transactions-job";
            public const string MetamaskConfirmationJob = "metamask-confirmation-job";
            public const string KangaMarketFetchJob = "kanga-market-fetch-job";
            public const string ScanPurchaseOperationsJob = "scan-purchase-operations-job";
            public const string FetchRahimCoinPriceJob = "fetch-rahimcoin-price-job";
            public const string ConfirmBinanceTransactionJob = "confirm-binance-job";
            public const string ExpireTokenLocksBackgroundJob = "expire-token-locks-job";
        }
    }
}