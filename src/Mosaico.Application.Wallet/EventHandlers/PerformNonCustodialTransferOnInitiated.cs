using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Base.Tools;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Nethereum.RPC.Accounts;
using Serilog;

namespace Mosaico.Application.Wallet.EventHandlers
{
    [EventInfo(nameof(PerformNonCustodialTransferOnInitiated), "wallets:api")]
    [EventTypeFilter(typeof(NonCustodialTransactionInitiated))]
    public class PerformNonCustodialTransferOnInitiated : EventHandlerBase
    {
        private readonly IEthereumClientFactory _ethereumClient;
        private readonly ILogger _logger;
        private readonly IDateTimeProvider _timeProvider;
        private readonly ITokenService _tokenService;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IWalletDispatcher _walletDispatcher;
        private readonly IUserManagementClient _managementClient;
        private readonly IUserWalletService _userWalletService;
        private readonly IOperationService _operationService;
        private readonly ICompanyWalletService _companyWalletService;
        private readonly ITokenLockService _lockService;

        public PerformNonCustodialTransferOnInitiated(IWalletDbContext walletDbContext, ILogger logger,
            IEthereumClientFactory ethereumClient, ITokenService tokenService, IWalletDispatcher walletDispatcher,
            IDateTimeProvider timeProvider, IUserManagementClient managementClient, IUserWalletService userWalletService, ICompanyWalletService companyWalletService, IOperationService operationService, ITokenLockService lockService)
        {
            _walletDbContext = walletDbContext;
            _logger = logger;
            _ethereumClient = ethereumClient;
            _tokenService = tokenService;
            _walletDispatcher = walletDispatcher;
            _timeProvider = timeProvider;
            _managementClient = managementClient;
            _userWalletService = userWalletService;
            _companyWalletService = companyWalletService;
            _operationService = operationService;
            _lockService = lockService;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var transactionEvent = @event?.GetData<NonCustodialTransactionInitiated>();
            if (transactionEvent != null)
                using (var dbTransaction = _walletDbContext.BeginTransaction())
                {
                    var operation = await _walletDbContext.Operations.FirstOrDefaultAsync(o => o.Id == transactionEvent.OperationId);
                    if (operation == null) return;
                    
                    var transaction =
                            await _walletDbContext.Transactions.FirstOrDefaultAsync(t =>
                                t.Id == transactionEvent.TransactionId);

                    if (transaction == null ||
                        transaction.Status.Key != Domain.Wallet.Constants.TransactionStatuses.Pending ||
                        transaction.Type?.Key != Domain.Wallet.Constants.TransactionType.Transfer) return;

                    var tokenLock = await _walletDbContext.TokenLocks.FirstOrDefaultAsync(t => t.LockReason == Constants.LockReasons.TRANSFER &&
                        t.Amount == transaction.TokenAmount && t.UserId == operation.UserId && !t.Expired);
                    
                    try
                    {
                        var transactionHash = string.Empty;
                        if (!transaction.TokenAmount.HasValue || transaction.TokenAmount <= 0)
                            throw new InvalidTokenAmountException();
                        
                        var account = await GetExecutiveAccountAsync(transaction);
                        var client = _ethereumClient.GetClient(transaction.Network);

                        if (transaction.PaymentCurrency?.NativeChainCurrency == true)
                        {
                            transactionHash = await client.TransferFundsWithoutReceiptAsync(account, transaction.To,
                                transaction.TokenAmount.Value);
                        }
                        else
                        {
                            var contractAddress = await GetContractAddressAsync(transaction);
                            if (string.IsNullOrWhiteSpace(contractAddress)) return;
                            await _userWalletService.AddTokenToWalletAsync(transaction.To, contractAddress, transaction.Network);
                            await _companyWalletService.AddTokenToWalletAsync(transaction.To, contractAddress, transaction.Network);
                            transactionHash = await _tokenService.TransferWithoutReceiptAsync(transaction.Network, account,
                                contractAddress, transaction.To, transaction.TokenAmount.Value);
                        }
                        await _operationService.SetTransactionInProgress(operation.Id, transactionId: transaction.Id, hash:transactionHash);
                        var receipt = await client.GetTransactionAsync(transactionHash);
                        if (receipt.Status != 1)
                        {
                            //TODO: to custom exception
                            throw new Exception($"Transaction failed");
                        }
                        await SetTransactionSuccessAsync(transaction, transactionHash);
                        await _walletDispatcher.SentCurrency(transaction.UserId, transaction.Id);
                        await _operationService.SetTransactionOperationCompleted(operation.Id, payed: transaction.PayedAmount, hash:transactionHash);
                        
                    }
                    catch (Exception ex)
                    {
                        _logger?.Error(ex, "Error during transfer");
                        await _walletDispatcher.SentCurrencyFailed(transaction.UserId, ex.Message);
                        await SetTransactionFailedAsync(transaction, ex.Message);
                        await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                    }
                    if (tokenLock != null)
                    {
                        await _lockService.SetExpiredAsync(tokenLock);
                    }
                    await dbTransaction.CommitAsync();
                }
        }

        private async Task SetTransactionSuccessAsync(Transaction transaction, string transactionHash)
        {
            var successStatus = await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(t =>
                t.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed);
            transaction.SetStatus(successStatus);
            transaction.TransactionHash = transactionHash;
            transaction.Currency = Constants.FIATCurrencies.USD;
            transaction.FinishedAt = _timeProvider.Now();
            var ticker = transaction.PaymentCurrency?.Ticker;
            var exchangeRate = 0m;
            if (string.IsNullOrWhiteSpace(ticker))
            {
                var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == transaction.TokenId.Value);
                ticker = token?.Symbol;
            }

            if (!string.IsNullOrWhiteSpace(ticker))
            {
                exchangeRate = await _walletDbContext.ExchangeRates
                    .OrderByDescending(er => er.CreatedAt)
                    .Where(t => t.Ticker == ticker).Select(t => t.Rate)
                    .FirstOrDefaultAsync();
            }
            transaction.PayedAmount = transaction.TokenAmount.Value * exchangeRate;
            await _walletDbContext.SaveChangesAsync();
        }

        private async Task SetTransactionFailedAsync(Transaction transaction, string error)
        {
            var failedStatus =
                await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(t =>
                    t.Key == Domain.Wallet.Constants.TransactionStatuses.Failed);
            transaction.FinishedAt = _timeProvider.Now();
            transaction.SetStatus(failedStatus);
            transaction.FailureReason = error;
            await _walletDbContext.SaveChangesAsync();
        }

        private async Task<string> GetContractAddressAsync(Transaction transaction)
        {
            var contractAddress = string.Empty;
            if (transaction.TokenId.HasValue)
            {
                var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == transaction.TokenId.Value);
                if (token == null) 
                    throw new TokenNotFoundException(transaction.TokenId.Value);

                contractAddress = token.Address;
            }
            else if (transaction.PaymentCurrency != null)
            {
                contractAddress = transaction.PaymentCurrency.ContractAddress;
            }

            if (string.IsNullOrWhiteSpace(contractAddress))
                throw new UnsupportedPaymentCurrencyException(contractAddress);

            return contractAddress;
        }

        private async Task<IAccount> GetExecutiveAccountAsync(Transaction transaction)
        {
            var privateKey = string.Empty;
            var wallet = await _walletDbContext.Wallets.FirstOrDefaultAsync(w =>
                w.AccountAddress == transaction.WalletAddress && transaction.Network == w.Network);
            if (wallet != null)
            {
                privateKey = wallet.PrivateKey;
                if (transaction.UserId != wallet.UserId)
                    throw new WalletNotFoundException(transaction.WalletAddress);
            }
            else
            {
                var companyWallet = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(c =>
                    c.AccountAddress == transaction.WalletAddress && transaction.Network == c.Network);
                if (companyWallet != null)
                {
                    privateKey = companyWallet.PrivateKey;
                    var permissions = await _managementClient.GetUserPermissionsAsync(transaction.UserId,
                        companyWallet.CompanyId, CancellationToken.None);
                    if (!permissions.Any(t => t.Key == Authorization.Base.Constants.Permissions.Company.CanEditDetails))
                    {
                        throw new WalletNotFoundException(transaction.WalletAddress);
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(privateKey))
                throw new WalletNotFoundException(transaction.WalletAddress);

            var client = _ethereumClient.GetClient(transaction.Network);
            var account = await client.GetAccountAsync(privateKey);
            if (account == null) throw new WalletNotFoundException(transaction.WalletAddress);

            return account;
        }
    }
}