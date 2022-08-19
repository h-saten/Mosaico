using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Nethereum.RPC.Accounts;
using Newtonsoft.Json;
using Serilog;

namespace Mosaico.Tools.CommandLine.Commands
{
    // export-project-wallet --projectId=148bc504-a080-4c54-2fa7-08da1c66d1da --wallet=0xb119e7A0002ee4500491cB8252033beCE835f48f --network=Polygon --dry=true
    [Command("export-project-wallet")]
    public class ExportProjectWalletCommand : CommandBase
    {
        private Guid _projectId;
        private string _network;
        private string _destinationWallet;
        private bool _isDryRun;
        private string _startFrom;
        private readonly IIdentityContext _identityContext;
        private readonly IUserWalletService _userWalletService;
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;
        
        private readonly IWalletDbContext _walletDbContext;

        public ExportProjectWalletCommand(IWalletDbContext walletDbContext, ILogger logger, IIdentityContext identityContext, IUserWalletService userWalletService, IEthereumClientFactory ethereumClientFactory, ITokenService tokenService)
        {
            _walletDbContext = walletDbContext;
            _logger = logger;
            _identityContext = identityContext;
            _userWalletService = userWalletService;
            _ethereumClientFactory = ethereumClientFactory;
            _tokenService = tokenService;
            SetOption("--projectId", "Project ID", s => _projectId = Guid.Parse(s));
            SetOption("--wallet", "Desitnation wallet", s => _destinationWallet = s);
            SetOption("--network", "Wallet network", s => _network = s);
            SetOption("--dry", "Is dry run", s => _isDryRun = string.IsNullOrWhiteSpace(s) || bool.Parse(s));
            SetOption("--startFrom", "Account address from which  to start export", s => _startFrom = s);
        }

        public override async Task Execute()
        {
            var projectWallet = await _walletDbContext.ProjectWallets.Include(w => w.Accounts).FirstOrDefaultAsync(w => w.ProjectId == _projectId && w.Network == _network);
            if (projectWallet == null) throw new ProjectWalletNotFoundException(_network, _projectId);
            _logger?.Information($"Project wallet was found");
            var paymentCurrencies = await _walletDbContext.PaymentCurrencies.Where(t => t.Chain == _network).ToListAsync();
            if (!paymentCurrencies.Any())
            {
                _logger?.Error($"No payment currencies was found.");
                throw new Exception($"No payment currencies");
            }
            var totalRaisedMoney = paymentCurrencies.ToDictionary(t => t.Ticker, t => 0m);
            var totalRaisedByUser = new Dictionary<string, Dictionary<string, decimal>>();
            var canContinue = false;
            foreach (var account in projectWallet.Accounts.OrderByDescending(a => a.Address))
            {
                if (string.IsNullOrWhiteSpace(_startFrom) ||
                    account.Address.ToLowerInvariant() == _startFrom.ToLowerInvariant())
                {
                    
                    canContinue = true;
                }
                if(!canContinue) continue;
                var user = await _identityContext.Users.FirstOrDefaultAsync(t => t.Id == account.UserId);
                if (user == null)
                {
                    _logger?.Error($"User {account.UserId} not found. Skipping...");
                    continue;
                }
                _logger?.Information($"Starting export from account {account.Address} that belongs to {user.FirstName} {user.LastName} / {user.Email}");
                foreach (var paymentCurrency in paymentCurrencies)
                {
                    var balance = 0m;
                    if(paymentCurrency.NativeChainCurrency) 
                    {
                        balance = await _userWalletService.NativePaymentCurrencyBalanceAsync(account.Address, paymentCurrency.Ticker, _network);
                    }
                    else
                    {
                        balance = await _userWalletService.PaymentCurrencyBalanceAsync(account.Address, paymentCurrency.Ticker, _network);
                    }

                    if (balance > 0.01m)
                    {
                        if (!totalRaisedByUser.ContainsKey(user.Email))
                        {
                            totalRaisedByUser.Add(user.Email, paymentCurrencies.ToDictionary(t => t.Ticker, t => 0m));
                        }
                        _logger?.Information($"- {balance} {paymentCurrency.Ticker} was invested by {user.FirstName} {user.LastName} ({user.Email})");
                        totalRaisedMoney[paymentCurrency.Ticker] += balance;
                        totalRaisedByUser[user.Email][paymentCurrency.Ticker] += balance;
                        if (!_isDryRun)
                        {
                            await ExportAssetsAsync(projectWallet, account, paymentCurrency, balance);
                        }
                    }
                }
                _logger?.Information("-------------------------------------------------------------");
            }
            _logger?.Information($"TOTAL RAISED ASSETS: ");
            foreach (var asset in totalRaisedMoney)
            {
                _logger?.Information($"{asset.Key}: {asset.Value}");
            }

            WriteInfoToJSON(totalRaisedByUser);
            _logger?.Information("Successfully exported data to json");
            Console.ReadLine();
        }
        
        private void WriteInfoToJSON(Dictionary<string, Dictionary<string, decimal>> items)
        {
            var fileName = $"D:\\project-wallet-export_{Guid.NewGuid().ToString().Substring(8)}.json";
            if (File.Exists(fileName)) throw new Exception($"File already exists");
            File.WriteAllText(fileName, JsonConvert.SerializeObject(items));
        }
        
        private async Task ExportAssetsAsync(ProjectWallet wallet, ProjectWalletAccount account, PaymentCurrency currency, decimal balance)
        {
            try
            {
                _logger?.Information("Exporting...");
                var client = _ethereumClientFactory.GetClient(_network);
                var ethAccount = client.GetWalletAccount(wallet.Mnemonic, wallet.Password, account.AccountId);
                var transactionHash = string.Empty;
                if (currency.NativeChainCurrency)
                {
                    transactionHash = await PerformNativeCurrencyTransferAsync(client, ethAccount, balance);
                }
                else
                {
                    transactionHash = await PerformTokenTransferAsync(client, ethAccount, currency, balance);
                }

                _logger?.Information(
                    $"Successfully transferred {balance} {currency.Ticker} to {_destinationWallet}. Transaction - {transactionHash}");
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, "Error during transfer");
            }
        }

        private async Task<string> PerformTokenTransferAsync(IEthereumClient client, IAccount account, PaymentCurrency currency, decimal balance)
        {
            var estimate = 0.003m;
            var gasFeeTransaction = await client.TransferFundsAsync(account.Address, estimate);
            _logger?.Information($"Successfully sent {estimate} gas to {account.Address} via {gasFeeTransaction}");
            var transactionHash = await _tokenService.TransferAsync(_network, account, currency.ContractAddress,
                _destinationWallet, balance);
            return transactionHash;
        }

        private async Task<string> PerformNativeCurrencyTransferAsync(IEthereumClient client, IAccount account, decimal balance)
        {
            var estimate = 0.003m;
            var gasFeeTransaction = await client.TransferFundsAsync(account.Address, estimate);
            _logger?.Information($"Successfully sent {estimate} gas to {account.Address} via {gasFeeTransaction}");
            var transactionHash = await client.TransferFundsAsync(account, _destinationWallet, balance);
            return transactionHash;
        }
    }
}