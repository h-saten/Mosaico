using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.ProjectManagement.Permissions;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Base.Tools;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Nethereum.RPC.Accounts;

namespace Mosaico.Application.Wallet.Services
{
    public class CrowdsalePurchaseService : ICrowdsalePurchaseService
    {
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IEthereumClientFactory _ethereumClient;
        private readonly ITokenService _tokenService;
        private readonly IUserManagementClient _managementClient;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserWalletService _userWalletService;
        private readonly IOperationService _operationService;

        public CrowdsalePurchaseService(IProjectManagementClient projectManagementClient, IWalletDbContext walletDbContext, ITokenService tokenService, IEthereumClientFactory ethereumClient, IProjectPermissionFactory permissionFactory, IUserManagementClient managementClient, IDateTimeProvider dateTimeProvider, IUserWalletService userWalletService, IOperationService operationService)
        {
            _projectManagementClient = projectManagementClient;
            _walletDbContext = walletDbContext;
            _tokenService = tokenService;
            _ethereumClient = ethereumClient;
            _managementClient = managementClient;
            _dateTimeProvider = dateTimeProvider;
            _userWalletService = userWalletService;
            _operationService = operationService;
        }
        
        private async Task<int> CountUserRefundedTransactions(string userId, Guid projectId)
        {
            var todayStart = _dateTimeProvider.Now().Date;
            var todayEnd = todayStart.AddDays(1).AddSeconds(-1);
            return await _walletDbContext.Transactions.Include(t => t.Status)
                .Where(t => t.UserId == userId && t.ProjectId == projectId && t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Refunded)
                .CountAsync(t => t.UserId == userId && t.CreatedAt.Date >= todayStart && t.CreatedAt.Date <= todayEnd);
        }

        private async Task<int> CountUserTransactionsAsync(string userId, Guid projectId)
        {
            var todayStart = _dateTimeProvider.Now().Date;
            var todayEnd = todayStart.AddDays(1).AddSeconds(-1);
            return await _walletDbContext.Transactions.Include(t => t.Status)
                .Where(t => t.UserId == userId && t.ProjectId == projectId && (t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Pending ||
                                                                               t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed ||
                                                                               t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Refunded ||
                                                                               t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.InProgress))
                .CountAsync(t => t.UserId == userId && t.CreatedAt.Date >= todayStart && t.CreatedAt.Date <= todayEnd);
        }

        public async Task<string> RefundPaymentAsync(Transaction transaction, decimal amount)
        {
            var client = _ethereumClient.GetClient(transaction.Network);
            var account = await GetRefundAccountAsync(transaction);
            if (transaction.PaymentCurrency.NativeChainCurrency)
            {
                // var gasEstimate = await client.GetTransferEstimateAsync(null, transaction.To);
                // var gasCompensation = (gasEstimate.TransactionFeeInETH * 0.2m) + gasEstimate.TransactionFeeInETH;
                var gasCompensation = 0.003m;
                await client.TransferFundsAsync(transaction.IntermediateAddress, gasCompensation);
                return await client.TransferFundsAsync(transaction.To, amount);
            }
            else
            {
                var contractAddress = transaction.PaymentCurrency?.ContractAddress;
                if (string.IsNullOrWhiteSpace(contractAddress))
                {
                    throw new Exception("Contract address is missing on transaction");
                }
                // var gasEstimate = await _tokenService.EstimateTransferAsync(transaction.Network, contractAddress, transaction.To);
                // var gasCompensation = (gasEstimate.TransactionFeeInETH * 0.2m) + gasEstimate.TransactionFeeInETH;
                var gasCompensation = 0.003m;
                await client.TransferFundsAsync(transaction.IntermediateAddress, gasCompensation);
                return await _tokenService.TransferAsync(transaction.Network, account, contractAddress, transaction.To, amount);
            }
        }

        public async Task<bool> CanPurchaseAsync(string userId, decimal tokenAmount, Guid stageId, string paymentMethod = null, bool ignoreTransactionCount = false)
        {
            var user = await _managementClient.GetUserAsync(userId);
            if (user == null)
            {
                return false;
            }

            if (paymentMethod != Domain.ProjectManagement.Constants.PaymentMethods.CreditCard && !user.IsAMLVerified && 
                paymentMethod != Domain.ProjectManagement.Constants.PaymentMethods.KangaExchange && 
                paymentMethod != Domain.ProjectManagement.Constants.PaymentMethods.Binance)
            {
                return false;
            }
            
            var currentStage = await _projectManagementClient.GetStageAsync(stageId);
            if (currentStage == null || (currentStage.Status != Domain.ProjectManagement.Constants.StageStatuses.Active && paymentMethod != Domain.ProjectManagement.Constants.PaymentMethods.BankTransfer))
            {
                return false;
            }
            
            if (currentStage.IsPrivate || currentStage.IsPreSale)
            {
                var investors = await _projectManagementClient.GetProjectPrivateInvestorsAsync(currentStage.ProjectId);
                if (!investors.Any(i => i.IsAllowed && i.StageId == currentStage.Id))
                {
                    return false;
                }
            }

            if (!ignoreTransactionCount)
            {
                var transactionCount = paymentMethod != Domain.ProjectManagement.Constants.PaymentMethods.Metamask
                    ? await CountUserTransactionsAsync(userId, currentStage.ProjectId)
                    : await CountUserRefundedTransactions(userId, currentStage.ProjectId);

                if (transactionCount >= 5)
                {
                    throw new ExceedsTransactionLimitException("Exceeds limit of transactions");
                }
            }

            var allStageTransactions = await _walletDbContext.Transactions
                .Where(t => t.StageId == currentStage.Id && t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed)
                .ToListAsync();
            
            var limit = currentStage.PurchaseLimits?.FirstOrDefault(l => paymentMethod != null && l.PaymentMethod == paymentMethod);
            
            var minPurchase = limit?.MinimumPurchase ?? currentStage.MinimumPurchase;
            var maxPurchase = limit?.MaximumPurchase ?? currentStage.MaximumPurchase;
            
            var sumPurchasedTokens = allStageTransactions.Sum(t => t.TokenAmount.Value);
            var airdrops = await _projectManagementClient.GetStageAirdropsAsync(stageId);
            foreach (var airdrop in airdrops)
            {
                if (currentStage != null)
                {
                    sumPurchasedTokens += airdrop.TotalCap;
                    sumPurchasedTokens += airdrop.TotalCap * currentStage.TokenPrice;
                }
            }
            
            var sumPurchasedUserTokens = allStageTransactions.Where(t => t.UserId == userId).Sum(t => t.TokenAmount.Value);
            if (sumPurchasedTokens + tokenAmount > currentStage.TokensSupply || sumPurchasedUserTokens + tokenAmount > maxPurchase || tokenAmount < minPurchase)
            {
                return false;
            }

            return true;
        }

        public async Task<string> WithdrawPaymentAsync(Domain.Wallet.Entities.Wallet wallet, Transaction transaction, decimal amount)
        {
            var client = _ethereumClient.GetClient(transaction.Network);
            if (transaction.PaymentCurrency.NativeChainCurrency)
            {
                var currentUserBalance = await _userWalletService.NativePaymentCurrencyBalanceAsync(
                    wallet.AccountAddress, transaction.PaymentCurrency.Ticker, transaction.PaymentCurrency.Chain);
                if (amount > currentUserBalance) throw new InsufficientFundsException(wallet.AccountAddress);
                
                // var gasEstimate = await client.GetTransferEstimateAsync(null, transaction.To);
                // var gasCompensation = (gasEstimate.TransactionFeeInETH * 0.2m) + gasEstimate.TransactionFeeInETH;
                // if (gasCompensation > 0.01m) throw new GasTooExpensiveException();
                var gasCompensation = 0.003m;
                await client.TransferFundsAsync(wallet.AccountAddress, gasCompensation);
                return await _userWalletService.TransferNativeCurrencyAsync(wallet, transaction.IntermediateAddress, amount);
            }
            else
            {
                var contractAddress = transaction.PaymentCurrency?.ContractAddress;
                if (string.IsNullOrWhiteSpace(contractAddress))
                {
                    throw new Exception("Contract address is missing on transaction");
                }
                var currentUserBalance = await _userWalletService.PaymentCurrencyBalanceAsync(
                    wallet.AccountAddress, transaction.PaymentCurrency.Ticker, transaction.PaymentCurrency.Chain);
                if (amount > currentUserBalance) throw new InsufficientFundsException(wallet.AccountAddress);
                //var gasEstimate = await _tokenService.EstimateTransferAsync(transaction.Network, contractAddress, transaction.To);
                // var gasCompensation = (gasEstimate.TransactionFeeInETH * 0.3m) + gasEstimate.TransactionFeeInETH;
                // if (gasCompensation > 0.01m) throw new GasTooExpensiveException();
                var gasCompensation = 0.003m;
                await client.TransferFundsAsync(wallet.AccountAddress, gasCompensation);
                return await _userWalletService.TransferTokenAsync(wallet, contractAddress, transaction.IntermediateAddress, amount);
            }
        }

        public async Task<string> TransferTargetTokensToUserAsync(Transaction transaction)
        {
            var operation = await _walletDbContext.Operations.FirstOrDefaultAsync(t =>
                t.Type == BlockchainOperationType.TRANSACTION_CONFIRMATION &&
                (t.State == OperationState.PENDING || t.State == OperationState.IN_PROGRESS || t.State == OperationState.SUCCESSFUL) &&
                t.TransactionId == transaction.Id);
            if(operation != null) return operation.TransactionHash;
            operation = await _operationService.CreateTransactionConfirmationAsync(transaction.Network, transaction.Id, transaction.ProjectId, transaction.WalletAddress);
            
            var account = await GetExecutiveAccountAsync(transaction);
            var contractAddress = await GetContractAddressAsync(transaction);
            if (string.IsNullOrWhiteSpace(contractAddress))
            {
                throw new Exception("Contract address is missing on transaction");
            }

            try
            {
                var hash = await _tokenService.TransferWithoutReceiptAsync(transaction.Network, account,
                    contractAddress, transaction.To, transaction.TokenAmount.Value);
                await _operationService.SetTransactionInProgress(operation.Id, hash);
                var client = _ethereumClient.GetClient(transaction.Network);
                var receipt = await client.GetTransactionAsync(hash);
                if (receipt.Status != 1)
                {
                    throw new Exception($"Transaction failed on blockchain");
                }

                await _operationService.SetTransactionOperationCompleted(operation.Id);
                return hash;
            }
            catch (Exception ex)
            {
                await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                throw;
            }
        }
        
        public async Task AddTokenToUserWalletAsync(Guid tokenId, string walletAddress, string network)
        {
            var wallet = await _walletDbContext.Wallets.FirstOrDefaultAsync(w => w.AccountAddress == walletAddress && w.Network == network);
            if (wallet != null && !wallet.Tokens.Any(t => t.TokenId == tokenId))
            {
                wallet.Tokens.Add(new WalletToToken
                {
                    TokenId = tokenId,
                    WalletId = wallet.Id
                });
                await _walletDbContext.SaveChangesAsync();
            }
        }
        
        public async Task<string> GetContractAddressAsync(Transaction transaction)
        {
            var contractAddress = string.Empty;
            if (transaction.TokenId.HasValue)
            {
                var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == transaction.TokenId.Value);
                if (token == null) 
                    throw new TokenNotFoundException(transaction.TokenId.Value);

                contractAddress = token.Address;
            }

            if (string.IsNullOrWhiteSpace(contractAddress))
                throw new UnsupportedPaymentCurrencyException(contractAddress);

            return contractAddress;
        }

        public async Task<IAccount> GetExecutiveAccountAsync(Transaction transaction)
        {
            var companyWallet = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(c =>
                c.AccountAddress == transaction.From && transaction.Network == c.Network);
            var privateKey = companyWallet.PrivateKey;
            
            if (string.IsNullOrWhiteSpace(privateKey))
                throw new WalletNotFoundException(transaction.WalletAddress);

            var client = _ethereumClient.GetClient(transaction.Network);
            var account = await client.GetAccountAsync(privateKey);
            if (account == null) throw new WalletNotFoundException(transaction.WalletAddress);

            return account;
        }
        
        public async Task<IAccount> GetRefundAccountAsync(Transaction transaction)
        {
            var projectWallet = await _walletDbContext.ProjectWalletAccounts.FirstOrDefaultAsync(c =>
                c.Address == transaction.IntermediateAddress && transaction.Network == c.ProjectWallet.Network);
            
            if (string.IsNullOrWhiteSpace(projectWallet.ProjectWallet.Mnemonic))
                throw new WalletNotFoundException(transaction.WalletAddress);
            
            var client = _ethereumClient.GetClient(transaction.Network);
            var account = client.GetWalletAccount(projectWallet.ProjectWallet.Mnemonic, projectWallet.ProjectWallet.Password, projectWallet.AccountId);
            if (account == null) throw new WalletNotFoundException(transaction.WalletAddress);
            return account;
        }
    }
}