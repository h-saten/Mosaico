using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DAL;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.TransactionsConfirmationJob, IsRecurring = true, Cron = "* */1 * * *")]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    public class TransactionsConfirmationJob : HangfireBackgroundJobBase
    {
        private readonly ILogger _logger;
        private readonly IWalletDbContext _walletDbContext;
        private readonly ITokenRepository _tokenRepository;
        private readonly ITransactionsRepository _transactionsRepository;
        
        public TransactionsConfirmationJob(
            IWalletDbContext walletDbContext,
            ITokenRepository tokenRepository, 
            ITransactionsRepository transactionsRepository, ILogger logger = null)
        {
            _walletDbContext = walletDbContext;
            _tokenRepository = tokenRepository;
            _transactionsRepository = transactionsRepository;
            _logger = logger;
        }
        
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        public override async Task ExecuteAsync(object parameters = null)
        {
            await Task.CompletedTask;
            var confirmedStatus = await _walletDbContext.TransactionStatuses
                .Where(m => m.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed)
                .SingleOrDefaultAsync();
    
            if (confirmedStatus is null)
            {
                // Should stop execution of this job because every next attempt also fail
                throw new TransactionStatusNotExistException(Domain.Wallet.Constants.TransactionStatuses.Confirmed);
            }
    
            var unconfirmedTransactions = await _transactionsRepository.TransactionsWaitingForConfirmationAsync();
    
            var oldestConfirmedTransactionHash = await _walletDbContext
                .Transactions
                .AsNoTracking()
                .Where(m => m.StatusId == confirmedStatus.Id)
                .OrderByDescending(t => t.LastConfirmationAttemptedAt)
                .Select(t => t.TransactionHash)
                .FirstOrDefaultAsync();
            
            var lastBlockNumber = await _transactionsRepository.TransactionBlockNumberByHashAsync(Blockchain.Base.Constants.BlockchainNetworks.Ethereum, oldestConfirmedTransactionHash);
            var groupedUnconfirmedTransactions = unconfirmedTransactions.GroupBy(m => m.PaymentCurrency.ContractAddress);
            foreach (var transactionGroup in groupedUnconfirmedTransactions)
            {
                var paymentCurrencyAddress = transactionGroup.Key;
    
                // TODO restrict to any block number, maybe estimate block number using time span between job execution
                var transfers = await _tokenRepository
                    .Erc20TransfersAsync(paymentCurrencyAddress, Blockchain.Base.Constants.BlockchainNetworks.Ethereum, lastBlockNumber);
                
                _logger?.Information($"[BLOCKCHAIN][{nameof(TransactionsConfirmationJob)}] For contract: '{paymentCurrencyAddress}' was found '{transfers.Count}' transactions.");
    
                if (transfers.Any() is false)
                {
                    continue;
                }
    
                foreach (var transaction in transactionGroup)
                {
                    await using var dbTransaction = _walletDbContext.BeginTransaction();
    
                    try
                    {
                        transaction.IncreaseConfirmationAttemptsCounter();
                        var transfer = transfers
                            .FirstOrDefault(t => t.Value == transaction.TokenAmount && string.Equals(t.ToAddress, transaction.To, StringComparison.InvariantCultureIgnoreCase));
    
                        if (transfer != null)
                        {
                            _logger?.Information($"[BLOCKCHAIN][{nameof(TransactionsConfirmationJob)}] Transaction for : '{transaction.WalletAddress}' ({transaction.TokenAmount}) was found.");
                            transaction.SetStatus(confirmedStatus);
                            transaction.TransactionHash = transfer.TransactionHash;
                            transaction.From = transfer.FromAddress;//ToDo check out if more infrastructure in need to populate sender and receiver
                            transaction.To = transfer.ToAddress;
                        }
                        
                        await _walletDbContext.SaveChangesAsync();
                        await dbTransaction.CommitAsync();
                    }
                    catch (Exception exception)
                    {
                        _logger?.Error($"Transaction: '{transaction.Id}' confirmation processing error. {exception.Message}");
                    }
                }
            }
        }
    }
}