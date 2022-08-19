using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Serilog;

namespace Mosaico.Tools.CommandLine.Commands
{
    public class UserBalanceDifferenceCsvModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Wallet { get; set; }
        public decimal PurchasedTokens { get; set; }
        public decimal Balance { get; set; }
        public decimal Difference { get; set; }
    }
    
    [Command("validate-transactions")]
    public class ValidateTransactionsCommand : CommandBase
    {
        private Guid _projectId;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IProjectDbContext _projectDbContext;
        private readonly IIdentityContext _identityContext;
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;
        
        public ValidateTransactionsCommand(IWalletDbContext walletDbContext, IProjectDbContext projectDbContext, ITokenService tokenService, ILogger logger, IIdentityContext identityContext)
        {
            _walletDbContext = walletDbContext;
            _projectDbContext = projectDbContext;
            _tokenService = tokenService;
            _logger = logger;
            _identityContext = identityContext;
            SetOption("-projectId", "project id", (s) => _projectId = Guid.Parse(s));
        }
        public override async Task Execute()
        {
            var project = await _projectDbContext.Projects.FirstOrDefaultAsync(p => p.Id == _projectId);
            if (project == null) throw new Exception($"Project not found");
            _logger?.Information($"Project {project.Title} was found");
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId);
            _logger?.Information($"Token {token.Name} was found");
            var transactions = await _walletDbContext.Transactions.Where(t => t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed && t.ProjectId == project.Id).ToListAsync();
            _logger?.Information($"Found {transactions.Count} transactions to verify...");
            var users = transactions.GroupBy(t => t.UserId);
            var itemsForExport = new List<UserBalanceDifferenceCsvModel>();
            foreach (var user in users)
            {
                var identity = await _identityContext.Users.FirstOrDefaultAsync(u => u.Id == user.Key);
                var userWallet = await _walletDbContext.Wallets.FirstOrDefaultAsync(w => w.UserId == user.Key);
                if (identity == null || userWallet == null)
                {
                    _logger?.Error($"User {user.Key} or his wallet was not found");
                    continue;
                }
                _logger?.Information($"Analyzing transactions for user {user.Key} - {identity.FirstName} {identity.LastName} ({identity.Email})");
                var purchasedTokens = user.Sum(t => t.TokenAmount.Value);
                var balance = await _tokenService.BalanceOfAsync(token.Network, token.Address, userWallet.AccountAddress);
                _logger?.Information($"User {user.Key} purchased {purchasedTokens} {token.Symbol} tokens and has {balance} {token.Symbol} on his account");
                if (balance - purchasedTokens > 0m)
                {
                    _logger?.Error($"User {user.Key} ({identity.Email}) has more {token.Symbol} tokens on his wallet {userWallet.AccountAddress} than purchased. The difference is {balance - purchasedTokens} {token.Symbol}");
                }
                itemsForExport.Add(new UserBalanceDifferenceCsvModel
                {
                    Balance = balance,
                    Difference = balance - purchasedTokens,
                    Email = identity.Email,
                    Name = $"{identity.FirstName} {identity.LastName}",
                    Phone = identity.PhoneNumber,
                    PurchasedTokens = purchasedTokens,
                    Wallet = userWallet.AccountAddress
                });
            }
            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                HasHeaderRecord = true
            };
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    using (var csv = new CsvWriter(streamWriter, csvConfiguration))
                    {
                        csv.WriteHeader<UserBalanceDifferenceCsvModel>();
                        await csv.NextRecordAsync();
                        await csv.WriteRecordsAsync(itemsForExport);
                    }
                }
                var content = memoryStream.ToArray();
                File.WriteAllBytes($"investor_balances_{Guid.NewGuid().ToString().Substring(0, 5)}.csv", content);
            }
        }
    }
}