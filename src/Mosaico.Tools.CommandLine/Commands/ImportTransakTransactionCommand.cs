using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Serilog;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("import-transak")]
    public class ImportTransakTransactionCommand : CommandBase
    {
        private readonly IWalletDbContext _walletContext;
        private readonly IProjectDbContext _dbContext;
        private readonly IFeeService _feeService;
        private readonly ILogger _logger;

        public ImportTransakTransactionCommand(IWalletDbContext walletDbContext, IProjectDbContext dbContext, IFeeService feeService, ILogger logger)
        {
            _walletContext = walletDbContext;
            _dbContext = dbContext;
            _feeService = feeService;
            _logger = logger;
        }

        public override async Task Execute()
        {
            var correlationId = "sgg4j9hgek4qwtd";
            var payedAmountInUSD = 107m;
            var userId = "167f0e14-ea8f-4f42-8f62-f20ef6a3bf81";
            var projectId = Guid.Parse("61da4b18-b18e-4def-8dba-08da1672c1c5");
            var cryptoCurrency = "USDC";
            var fiatCurrency = "EUR";
            var fiatAmount = 105.1m;
            var paymentOption = "credit_debit_card";
            
            var project = await _dbContext.Projects.FirstOrDefaultAsync(t => t.Id == projectId);
            var token = await _walletContext.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId);
            var currentSaleStage = project.ActiveStage();
            var type = await _walletContext.TransactionType.FirstOrDefaultAsync(t => t.Key == Domain.Wallet.Constants.TransactionType.Purchase);
            if (type == null)
            {
                throw new TransactionTypeNotFoundException(Domain.Wallet.Constants.TransactionType.Purchase);
            }

            var status = await _walletContext.TransactionStatuses.FirstOrDefaultAsync(t => t.Key == Domain.Wallet.Constants.TransactionStatuses.Pending);
            if (status == null)
            {
                throw new TransactionStatusNotExistException(Domain.Wallet.Constants.TransactionStatuses.Pending);
            }

            var companyWallet = await _walletContext.CompanyWallets.FirstOrDefaultAsync(c =>
                c.CompanyId == project.CompanyId && c.Network == token.Network);
            
            if (companyWallet == null)
            {
                throw new CompanyWalletNotFoundException(project.CompanyId.ToString());
            }
            
            var paymentCurrency = await _walletContext
                .PaymentCurrencies
                .Where(c => c.Ticker == cryptoCurrency && c.Chain == token.Network)
                .SingleOrDefaultAsync();
                
            if (paymentCurrency is null)
            {
                throw new UnsupportedCurrencyException(cryptoCurrency);
            }

            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w =>
                w.UserId == userId
                && w.Network == token.Network);
            if (wallet == null)
            {
                throw new WalletNotFoundException(userId);
            }

            var tokenAmount = payedAmountInUSD / currentSaleStage.TokenPrice;

            var fee = await _feeService.GetFeeAsync(project.Id, currentSaleStage.Id);

            var projectWallet = await _walletContext.ProjectWallets.FirstOrDefaultAsync(t => t.ProjectId == project.Id && t.Network == token.Network);
            var account = projectWallet.Accounts.FirstOrDefault(t => t.UserId == userId);

            var paymentTransaction = new Transaction
            {
                CorrelationId = correlationId,
                UserId = userId,
                TokenAmount = tokenAmount,
                TokenId = token.Id,
                PaymentProcessor = Payments.Transak.Constants.PaymentProcessorName,
                InitiatedAt = DateTimeOffset.UtcNow,
                WalletAddress = wallet.AccountAddress,
                Network = token.Network,
                PaymentCurrencyId = paymentCurrency.Id,
                Currency = fiatCurrency,
                PayedAmount = fiatAmount,
                PaymentCurrency = paymentCurrency,
                From = companyWallet.AccountAddress,
                To = wallet.AccountAddress,
                StageId = currentSaleStage.Id,
                IntermediateAddress = account.Address,
                PaymentMethod = paymentOption,
                ProjectId = project.Id,
                TokenPrice = currentSaleStage.TokenPrice,
                FeePercentage = fee
            };
        
            paymentTransaction.SetStatus(status);
            paymentTransaction.SetType(type);
            
            _walletContext.Transactions.Add(paymentTransaction);
            await _walletContext.SaveChangesAsync();
            
            _logger.Information($"Transaction imported successfully");
        }
    }
}