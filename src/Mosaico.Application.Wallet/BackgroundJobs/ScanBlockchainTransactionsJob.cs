using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.ValueObjects;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Models;
using Serilog;

namespace Mosaico.Application.Wallet.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.ScanBlockchainTransactionsJob, IsRecurring = true)]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    public class ScanBlockchainTransactionsJob : HangfireBackgroundJobBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IEtherScanner _etherScanner;
        private readonly IDisplayNameFinder _displayNameFinder;
        private readonly ILogger _logger;
        public ScanBlockchainTransactionsJob(IWalletDbContext walletDbContext, IDisplayNameFinder displayNameFinder, IEtherScanner etherScanner, ILogger logger)
        {
            _walletDbContext = walletDbContext;
            _displayNameFinder = displayNameFinder;
            _etherScanner = etherScanner;
            _logger = logger;
        }
        
        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public override async Task ExecuteAsync(object parameters = null)
        {
            var paymentCurrencies = await _walletDbContext.PaymentCurrencies.ToListAsync();
            
            var transferTransactionType =
                await _walletDbContext.TransactionType.FirstOrDefaultAsync(t =>
                    t.Key == Domain.Wallet.Constants.TransactionType.Transfer);
            
            var transactionStatus =
                await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(t =>
                    t.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed);

            var companyWallets = (await _walletDbContext.CompanyWallets
                .Include(c => c.Tokens)
                .ThenInclude(t => t.Token).ToListAsync()).Select((cw) => cw as IWallet)
                .Where(c => c != null);
            
            var userWallets = (await _walletDbContext.Wallets
                .Include(c => c.Tokens)
                .ThenInclude(t => t.Token).ToListAsync()).Select((cw) => cw as IWallet)
                .Where(c => c != null);;

            var take = 100;
            var skip = 0;
            var threadDelayInMs = 1000;
            
            var allWallets = companyWallets.Union(userWallets);
            var currentWallets = allWallets.Skip(skip).Take(take);
            
            while (currentWallets.Any())
            {
                foreach (var wallet in currentWallets)
                {
                    await ScanForNewTransactionsAsync(wallet, paymentCurrencies, transactionStatus, transferTransactionType);
                }
                
                skip += take;
                currentWallets = allWallets.Skip(skip).Take(take);
                Thread.Sleep(threadDelayInMs);
            }
        }

        private async Task ScanForNewTransactionsAsync(IWallet wallet, List<PaymentCurrency> currencies, TransactionStatus status, TransactionType type, CancellationToken token = new CancellationToken())
        {
            if (wallet != null)
            {
                foreach(var paymentCurrency in currencies)
                {
                    var allTransactions = 
                        paymentCurrency.NativeChainCurrency ? 
                            await _etherScanner.GetTransactionsAsync(paymentCurrency.Chain, wallet.AccountAddress, 0, 50, wallet.LastSyncBlockHash) :
                            await _etherScanner.GetERC20TransactionsAsync(paymentCurrency.Chain, wallet.AccountAddress, paymentCurrency.ContractAddress, 0, 50, wallet.LastSyncBlockHash);
                    await CreateOrUpdateTransactionsAsync(wallet, paymentCurrency, allTransactions.Result, type, status, token);
                }
            }
        }

        private async Task CreateOrUpdateTransactionsAsync(IWallet wallet, PaymentCurrency currency, List<ScanTransaction> transactions, TransactionType type, TransactionStatus status, CancellationToken token)
        {
            using (var t = _walletDbContext.BeginTransaction())
            {
                try
                {
                    foreach (var transaction in transactions.Where(t => !string.IsNullOrWhiteSpace(t.From) && !string.IsNullOrWhiteSpace(t.To)))
                    {
                        var value = new Wei(BigInteger.Parse(transaction.Value), currency.DecimalPlaces);
                        var transactionInDb = await _walletDbContext.Transactions.FirstOrDefaultAsync(
                            t => t.TransactionHash == transaction.Hash, cancellationToken: token);
                        var fromDisplayName = await _displayNameFinder.FindDisplayNameAsync(transaction.From, token);
                        var toDisplayName = await _displayNameFinder.FindDisplayNameAsync(transaction.To, token);

                        var exchangeRate = await _walletDbContext.ExchangeRates.AsNoTracking()
                            .OrderByDescending(e => e.CreatedAt)
                            .Where(t => t.Ticker == currency.Ticker)
                            .Select(e => e.Rate)
                            .FirstOrDefaultAsync(token);

                        if (transactionInDb == null)
                        {
                            _walletDbContext.Transactions.Add(new Transaction
                            {
                                From = transaction.From,
                                To = transaction.To,
                                Currency = exchangeRate > 0 ? Constants.FIATCurrencies.USD : currency.Ticker,
                                Network = currency.Chain,
                                Status = status,
                                StatusId = status.Id,
                                Type = type,
                                TypeId = type.Id,
                                FinishedAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(transaction.TimeStamp)),
                                PaymentProcessor = Constants.PaymentProcessors.Metamask,
                                TokenAmount = value.ToDecimal(),
                                TransactionHash = transaction.Hash,
                                PaymentCurrencyId = currency.Id,
                                PayedAmount = exchangeRate > 0 ? exchangeRate * value.ToDecimal() : value.ToDecimal(),
                                PaymentCurrency = currency,
                                WalletAddress = wallet.AccountAddress,
                                FromDisplayName = fromDisplayName,
                                ToDisplayName = toDisplayName
                            });
                        }
                        else
                        {
                            if (!transactionInDb.TokenAmount.HasValue)
                            {
                                transactionInDb.TokenAmount = value.ToDecimal();
                                transactionInDb.PayedAmount = value.ToDecimal();
                                transactionInDb.PaymentCurrency = currency;
                                transactionInDb.Currency = currency.Ticker;
                            }
                            transactionInDb.FromDisplayName = fromDisplayName;
                            transactionInDb.ToDisplayName = toDisplayName;
                        }
                    }

                    if (transactions.Any())
                    {
                        wallet.LastSyncBlockHash =
                            (transactions.Max(t => BigInteger.Parse(t.BlockNumber)) + 1).ToString();
                        await _walletDbContext.SaveChangesAsync(token);
                    }

                    await t.CommitAsync(token);
                }
                catch (Exception ex)
                {
                    _logger?.Warning($"Failure during transaction scanning - {ex.Message} / {ex.StackTrace}");
                    await t.RollbackAsync(token);
                }
            }
        }
    }
}