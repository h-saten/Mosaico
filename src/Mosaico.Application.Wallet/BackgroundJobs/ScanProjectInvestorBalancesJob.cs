using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Base.Tools;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Wallet.BackgroundJobs
{
    [BackgroundJob(Constants.Jobs.ScanProjectWalletBalanceJob, IsRecurring = true, Cron = "*/30 * * * *")]
    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
    public class ScanProjectInvestorBalancesJob : HangfireBackgroundJobBase
    {
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ITokenService _tokenService;
        private readonly IDateTimeProvider _provider;
        private readonly IUserManagementClient _managementClient;

        public ScanProjectInvestorBalancesJob(IProjectManagementClient projectManagementClient, IWalletDbContext walletDbContext, IEthereumClientFactory ethereumClientFactory, ITokenService tokenService, IUserManagementClient managementClient, IExchangeRateService exchangeRateService, IDateTimeProvider provider)
        {
            _projectManagementClient = projectManagementClient;
            _walletDbContext = walletDbContext;
            _ethereumClientFactory = ethereumClientFactory;
            _tokenService = tokenService;
            _managementClient = managementClient;
            _exchangeRateService = exchangeRateService;
            _provider = provider;
        }

        public override async Task ExecuteAsync(object parameters = null)
        {
            var timeThreshold = _provider.Now().AddMinutes(-30);
            var activeProjects = await _projectManagementClient.GetActiveProjectsAsync();
            if (activeProjects.Any())
            {
                var paymentCurrencies = await _walletDbContext.PaymentCurrencies.ToListAsync();
                var projectIds = activeProjects.Select(p => p.Id).ToArray();
                var projectWallets = await _walletDbContext.ProjectWallets.Include(pw => pw.Accounts).Where(pw => projectIds.Contains(pw.ProjectId)).ToListAsync();
                var projectWalletAccounts = projectWallets.SelectMany(pw => pw.Accounts);
                foreach (var walletAccount in projectWalletAccounts)
                {
                    var walletPaymentCurrencies = paymentCurrencies.Where(pc => pc.Chain == walletAccount.ProjectWallet.Network);
                    var whitelistedCurrencies = walletPaymentCurrencies.Select(pc => pc.Ticker).ToList();
                    whitelistedCurrencies.Add(Constants.FIATCurrencies.USD);
                    var exchangeRates = await _exchangeRateService.GetExchangeRatesAsync(whitelisted: whitelistedCurrencies);
                    var investor = await _walletDbContext.Investors.FirstOrDefaultAsync(i => i.ProjectId == walletAccount.ProjectWallet.ProjectId && i.UserId == walletAccount.UserId);
                    
                    if (investor == null)
                    {
                        investor = new Investor
                        {
                            UserId = walletAccount.UserId,
                            ProjectId = walletAccount.ProjectWallet.ProjectId
                        };
                        _walletDbContext.Investors.Add(investor);
                    }

                    if (!investor.LastUpdatedAt.HasValue || investor.LastUpdatedAt <= timeThreshold)
                    {
                        var investorBalances = investor.Balances;
                        foreach (var paymentCurrency in walletPaymentCurrencies)
                        {
                            await UpdateInvestorBalance(paymentCurrency, walletAccount, investorBalances);
                        }

                        await UpdateBankTransferValueAsync(investor.UserId, investor.ProjectId, investorBalances);

                        investor.TotalInvestment = 0;
                        
                        foreach (var balance in investorBalances)
                        {
                            var exchangeRate = exchangeRates.FirstOrDefault(er => er.Ticker == balance.Currency);
                            if (exchangeRate != null)
                            {
                                investor.TotalInvestment += balance.Balance * exchangeRate.Rate;
                            }
                        }

                        investor.Balances = investorBalances;
                        investor.LastUpdatedAt = _provider.Now();
                        await _walletDbContext.SaveChangesAsync();
                    }
                }
            }
        }

        private async Task UpdateBankTransferValueAsync(string userId, Guid projectId, List<InvestorBalance> investorBalances)
        {
            investorBalances.RemoveAll(b => b.Currency == "USD" && b.WalletAddress == null);
            var manualBankTransferAmount = await GetBankTransferAmountAsync(userId, projectId);
            if (manualBankTransferAmount.HasValue)
            {
                investorBalances.Add(new InvestorBalance
                {
                    Balance = manualBankTransferAmount.Value,
                    Currency = "USD",
                    Network = null,
                    WalletAddress = null
                });
            }
        }

        private Task<decimal?> GetBankTransferAmountAsync(string userId, Guid projectId)
        {
            return _walletDbContext.Transactions
                .Where(t => t.UserId == userId && t.ProjectId == projectId && t.PayedInUSD > 0 &&
                            t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed)
                .SumAsync(t => t.PayedInUSD);
        }

        private async Task UpdateInvestorBalance(PaymentCurrency paymentCurrency, ProjectWalletAccount walletAccount, List<InvestorBalance> investorBalances)
        {
            var balance = 0m;
            if (paymentCurrency.NativeChainCurrency)
            {
                var client = _ethereumClientFactory.GetClient(paymentCurrency.Chain);
                balance = await client.GetAccountBalanceAsync(walletAccount.Address);
            }
            else
            {
                balance = await _tokenService.BalanceOfAsync(paymentCurrency.Chain, paymentCurrency.ContractAddress, walletAccount.Address);
            }

            var investorBalance = investorBalances.FirstOrDefault(ib => ib.Network == paymentCurrency.Chain &&
                ib.Currency == paymentCurrency.Ticker && ib.WalletAddress == walletAccount.Address);
            if (investorBalance == null)
            {
                investorBalance = new InvestorBalance
                {
                    Currency = paymentCurrency.Ticker,
                    WalletAddress = walletAccount.Address,
                    Network = paymentCurrency.Chain
                };
                investorBalances.Add(investorBalance);
            }

            investorBalance.Balance = balance;
        }
    }
}