using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Nethereum.RPC.Accounts;
using Serilog;

namespace Mosaico.Tools.CommandLine.Commands
{
    public class TransactionModelCsv
    {
        [CsvHelper.Configuration.Attributes.Index(0)]
        public string CorrelationId { get; set; }
        
        [CsvHelper.Configuration.Attributes.Index(1)]
        public decimal PayedInPLN { get; set; }
        
        [CsvHelper.Configuration.Attributes.Index(2)]
        public decimal Tokens { get; set; }
        
        [CsvHelper.Configuration.Attributes.Index(3)]
        public decimal Price { get; set; }
        
        [CsvHelper.Configuration.Attributes.Index(4)]
        public string Email { get; set; }
        
        [CsvHelper.Configuration.Attributes.Index(5)]
        public string Project { get; set; }
    }
    
    [Command("accept-transactions")]
    public class AcceptTransactionsCommand : CommandBase
    {
        private string _fileLocation;
        private readonly IWalletDbContext _walletDbContext;
        private readonly ILogger _logger;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IFeeService _feeService;
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly ITokenService _tokenService;
        private readonly IProjectDbContext _projectDbContext;
        private readonly IIdentityContext _identityContext;

        public AcceptTransactionsCommand(IWalletDbContext walletDbContext, ILogger logger, IExchangeRateService exchangeRateService, IFeeService feeService, IEthereumClientFactory ethereumClientFactory, ITokenService tokenService, IProjectDbContext projectDbContext, IIdentityContext identityContext)
        {
            _walletDbContext = walletDbContext;
            _logger = logger;
            _exchangeRateService = exchangeRateService;
            _feeService = feeService;
            _ethereumClientFactory = ethereumClientFactory;
            _tokenService = tokenService;
            _projectDbContext = projectDbContext;
            _identityContext = identityContext;
            SetOption("-file", "File with list of transactions", s => _fileLocation = s);
        }
        public override async Task Execute()
        {
            if (!File.Exists(_fileLocation))
            {
                throw new Exception("File not found");
            }
            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                HasHeaderRecord = true
            };
            var file = File.ReadAllBytes(_fileLocation);
            using (var memoryStream = new MemoryStream(file))
            {
                using (var reader = new StreamReader(memoryStream))
                {
                    using (var csv = new CsvReader(reader, csvConfiguration))
                    {
                        var records = csv.GetRecords<TransactionModelCsv>().ToList();
                        foreach (var @record in records)
                        {
                            await ApproveTransactionAsync(@record);
                        }
                    }
                }
            }
            Console.ReadLine();
        }

        private async Task CreateTransactionAsync(TransactionModelCsv dto)
        {
            var statuses = await _walletDbContext.TransactionStatuses.ToListAsync();
            var rates = await _exchangeRateService.GetExchangeRatesAsync();
            var project = await _projectDbContext.Projects.FirstOrDefaultAsync(p => p.Title == dto.Project);
            var user = await _identityContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId);
            
            if (project == null)
            {
                _logger?.Error($"{dto.CorrelationId} Project {dto.Project} not found");
                return;
            }
            if (user == null)
            {
                _logger?.Error($"{dto.CorrelationId} User {dto.Email} not found");
                return;
            }
            var companyWallet = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(t =>
                t.Network == token.Network && t.CompanyId == project.CompanyId);
            var wallet = await _walletDbContext.Wallets.FirstOrDefaultAsync(w => w.Network == token.Network && w.UserId == user.Id);
            var type = await _walletDbContext.TransactionType.FirstOrDefaultAsync(t =>
                t.Key == Domain.Wallet.Constants.TransactionType.Purchase);
            var transaction = new Transaction
            {
                Currency = "PLN",
                CorrelationId = dto.CorrelationId,
                TokenAmount = dto.Tokens,
                TokenPrice = dto.Price,
                PayedAmount = dto.PayedInPLN,
                ProjectId = project.Id,
                UserId = user.Id,
                InitiatedAt = DateTimeOffset.UtcNow,
                Network = token.Network,
                TokenId = token.Id,
                To = wallet.AccountAddress,
                From = companyWallet.AccountAddress,
                WalletAddress = wallet.AccountAddress,
                PaymentMethod = "BANK_TRANSFER",
                PaymentProcessor = Domain.ProjectManagement.Constants.PaymentMethods.BankTransfer
            };
            transaction.SetType(type);
            var rate = rates.FirstOrDefault(r => r.Ticker == transaction.Currency);
            transaction.FeePercentage = await _feeService.GetFeeAsync(transaction.ProjectId.Value);
            transaction.ExchangeRate = rate.Rate;
            transaction.PayedInUSD = transaction.PayedAmount * rate.Rate;
            transaction.Fee = 0;
            transaction.FeeInUSD = 0;
            transaction.MosaicoFee = _feeService.GetMosaicoFeeValueAsync(transaction.FeePercentage, transaction.Fee, transaction.PayedAmount.Value);
            transaction.MosaicoFeeInUSD = transaction.MosaicoFee * rate.Rate;
            transaction.FinishedAt = DateTimeOffset.UtcNow;
            transaction.SetStatus(statuses.FirstOrDefault(s =>
                s.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed));
            _walletDbContext.Transactions.Add(transaction);
            await _walletDbContext.SaveChangesAsync();
            await AddTokenToUserWalletAsync(transaction.TokenId.Value, transaction.To, transaction.Network);
            var transactionHash = await TransferTargetTokensToUserAsync(transaction);
            _logger.Information($"Tokens were successfully sent to the investor. {transactionHash}");
            transaction.TransactionHash = transactionHash;
            _walletDbContext.Transactions.Update(transaction);
            await _walletDbContext.SaveChangesAsync();
        }

        private async Task ApproveTransactionAsync(TransactionModelCsv dto)
        {
            var transaction = await _walletDbContext.Transactions.FirstOrDefaultAsync(t => t.CorrelationId.ToLower() == dto.CorrelationId.ToLower());
            if (transaction == null)
            {
                _logger.Error($"Transaction not found");
                await CreateTransactionAsync(dto);
            }
            else
            {
                if (transaction.Status.Key != Domain.Wallet.Constants.TransactionStatuses.Pending)
                {
                    _logger.Error($"{dto.CorrelationId} Transaction is not pending");
                    return;
                }
                _logger.Information($"Transaction {transaction.CorrelationId} was found in database. Starting acceptance");
                using (var dbTransaction = _walletDbContext.BeginTransaction())
                {
                    try
                    {
                        var statuses = await _walletDbContext.TransactionStatuses.ToListAsync();
                        var rates = await _exchangeRateService.GetExchangeRatesAsync();
                        
                        transaction.TokenAmount = dto.Tokens;
                        transaction.TokenPrice = dto.Price;
                        transaction.PayedAmount = dto.PayedInPLN;
                        transaction.Currency = "PLN";
                        transaction.FinishedAt = DateTimeOffset.UtcNow;
                        transaction.SetStatus(statuses.FirstOrDefault(s =>
                            s.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed));
                        
                        var rate = rates.FirstOrDefault(r => r.Ticker == transaction.Currency);
                        transaction.ExchangeRate = rate.Rate;
                        transaction.PayedInUSD = transaction.PayedAmount * rate.Rate;
                        transaction.Fee = 0;
                        transaction.FeeInUSD = 0;
                        transaction.MosaicoFee = _feeService.GetMosaicoFeeValueAsync(transaction.FeePercentage, transaction.Fee, transaction.PayedAmount.Value);
                        transaction.MosaicoFeeInUSD = transaction.MosaicoFee * rate.Rate;
                        await AddTokenToUserWalletAsync(transaction.TokenId.Value, transaction.To, transaction.Network);
                        var transactionHash = await TransferTargetTokensToUserAsync(transaction);
                        _logger.Information($"Tokens were successfully sent to the investor. {transactionHash}");
                        transaction.TransactionHash = transactionHash;
                        await _walletDbContext.SaveChangesAsync();
                        await dbTransaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await dbTransaction.RollbackAsync();
                        _logger?.Error(ex, "Error during transaction acceptance");
                    }
                }
            }
        }
        
        private async Task<string> TransferTargetTokensToUserAsync(Transaction transaction)
        {
            var account = await GetExecutiveAccountAsync(transaction);
            var contractAddress = await GetContractAddressAsync(transaction);
            if (string.IsNullOrWhiteSpace(contractAddress))
            {
                throw new Exception("Contract address is missing on transaction");
            }
            return await _tokenService.TransferAsync(transaction.Network, account, contractAddress, transaction.To, transaction.TokenAmount.Value);
        }
        
        private async Task AddTokenToUserWalletAsync(Guid tokenId, string walletAddress, string network)
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
        
        private async Task<IAccount> GetExecutiveAccountAsync(Transaction transaction)
        {
            var companyWallet = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(c =>
                c.AccountAddress == transaction.From && transaction.Network == c.Network);
            var privateKey = companyWallet.PrivateKey;
            
            if (string.IsNullOrWhiteSpace(privateKey))
                throw new WalletNotFoundException(transaction.WalletAddress);

            var client = _ethereumClientFactory.GetClient(transaction.Network);
            var account = await client.GetAccountAsync(privateKey);
            if (account == null) throw new WalletNotFoundException(transaction.WalletAddress);

            return account;
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

            if (string.IsNullOrWhiteSpace(contractAddress))
                throw new UnsupportedPaymentCurrencyException(contractAddress);

            return contractAddress;
        }
    }
}