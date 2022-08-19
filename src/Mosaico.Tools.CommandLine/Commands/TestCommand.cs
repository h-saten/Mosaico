using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Extensions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Serilog;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("say-hello", "says hello to the name from options")]
    public class TestCommand : CommandBase
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _projectDbContext;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IUserWalletService _userWalletService;
        private readonly IFeeService _feeService;
        private readonly IProjectWalletService _projectWalletService;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private string _name;

        public TestCommand(ILogger logger,  IProjectDbContext projectDbContext, IWalletDbContext walletDbContext, IExchangeRateService exchangeRateService, IUserWalletService userWalletService, IFeeService feeService, IProjectWalletService projectWalletService, IEventFactory eventFactory, IEventPublisher eventPublisher)
        {
            _logger = logger;
            _projectDbContext = projectDbContext;
            _walletDbContext = walletDbContext;
            _exchangeRateService = exchangeRateService;
            _userWalletService = userWalletService;
            _feeService = feeService;
            _projectWalletService = projectWalletService;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            SetOption("-name", "Name of the person to say hi to", s => _name = s);
        }

        public override async Task Execute()
        {
            var request = new
            {
                PayedAmount = 435m,
                Currency = "MATIC",
                ProjectId = Guid.Parse("148bc504-a080-4c54-2fa7-08da1c66d1da")
            };
            var userId = "00ca77c7-a5ae-4a70-921f-165e12676fd1";
            
            var project = await _projectDbContext.GetProjectOrThrowAsync(request.ProjectId);
                    
            if (project == null)
                throw new ProjectNotFoundException(request.ProjectId);

            var correlationId = Guid.NewGuid().ToString();
            _logger?.Verbose($"New transaction correlation id {correlationId}");
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId);
            if (token == null)
            {
                _logger?.Fatal($"Potential breach. User tries to buy tokens when he is unauthorized");
                throw new TokenNotFoundException(project.TokenId.ToString());
            }
            
            var currentSaleStage = project.ActiveStage();
            if (currentSaleStage == null)
            {
                throw new ProjectStageNotExistException(project.Id);
            }
            var currency = await _walletDbContext
                .PaymentCurrencies
                .FirstOrDefaultAsync(x => x.Chain == token.Network && x.Ticker == request.Currency);
            
            if (currency == null) 
                throw new UnsupportedCurrencyException(request.Currency);
            
            var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(request.Currency);
            if (exchangeRate == null)
            {
                throw new InvalidExchangeRateException(request.Currency);
            }
            
            if (_walletDbContext.Transactions.Any(t => t.CorrelationId == correlationId))
            {
                throw new TransactionAlreadyExistsException(correlationId);
            }

            var payedInCurrency = request.PayedAmount;
            var paymentAmountInUSD = request.PayedAmount * exchangeRate.Rate;
            var tokenAmount = Math.Round(paymentAmountInUSD / currentSaleStage.TokenPrice, 2);

            var type = await _walletDbContext.TransactionType.FirstOrDefaultAsync(t => t.Key == Domain.Wallet.Constants.TransactionType.Purchase);
            if (type == null)
            {
                throw new TransactionTypeNotFoundException(Domain.Wallet.Constants.TransactionType.Purchase);
            }

            var status = await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(t => t.Key == Domain.Wallet.Constants.TransactionStatuses.Pending);
            if (status == null)
            {
                throw new TransactionStatusNotExistException(Domain.Wallet.Constants.TransactionStatuses.Pending);
            }

            var companyWallet = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(c =>
                c.CompanyId == project.CompanyId && c.Network == token.Network);
            
            if (companyWallet == null)
            {
                throw new CompanyWalletNotFoundException(project.CompanyId.ToString());
            }

            var wallet = await _walletDbContext.Wallets.FirstOrDefaultAsync(w =>
                w.UserId == userId
                && w.Network == token.Network);
            
            if (wallet == null)
            {
                throw new WalletNotFoundException(userId);
            }

            var walletBalance = currency.NativeChainCurrency ?
                await _userWalletService.NativePaymentCurrencyBalanceAsync(wallet.AccountAddress, currency.Ticker, wallet.Network) :
                await _userWalletService.PaymentCurrencyBalanceAsync(wallet.AccountAddress, currency.Ticker, wallet.Network);

            if (walletBalance < request.PayedAmount) throw new InsufficientFundsException(wallet.AccountAddress);
            
            var projectWallet =
                await _walletDbContext.ProjectWallets.FirstOrDefaultAsync(w =>
                    w.ProjectId == project.Id && w.Network == token.Network);
            if(projectWallet == null)
            {
                projectWallet = await _projectWalletService.CreateWalletAsync(token.Network, project.Id);
            }
            
            var fee = await _feeService.GetFeeAsync(project.Id, currentSaleStage.Id);
            var mosaicoFee = _feeService.GetMosaicoFeeValueAsync(fee, 0, payedInCurrency);
            var mosaicoFeeInUSD = mosaicoFee * exchangeRate.Rate;
            var account = await _projectWalletService.GetAccountAsync(token.Network, project.Id, userId);
            var paymentTransaction = new Transaction
            {
                CorrelationId = correlationId,
                UserId = userId,
                TokenAmount = tokenAmount,
                TokenId = token.Id,
                PaymentProcessor = Domain.ProjectManagement.Constants.PaymentMethods.MosaicoWallet,
                InitiatedAt = DateTimeOffset.UtcNow,
                WalletAddress = wallet.AccountAddress,
                Network = token.Network,
                PaymentCurrencyId = currency.Id,
                Currency = request.Currency,
                PayedAmount = payedInCurrency,
                PaymentCurrency = currency,
                From = companyWallet.AccountAddress,
                To = wallet.AccountAddress,
                StageId = currentSaleStage.Id,
                ProjectId = request.ProjectId,
                PayedInUSD = paymentAmountInUSD,
                IntermediateAddress = account.Address,
                PaymentMethod = "DEFAULT",
                TokenPrice = currentSaleStage.TokenPrice,
                FeePercentage = fee,
                Fee = 0,
                FeeInUSD = 0,
                MosaicoFee = mosaicoFee,
                MosaicoFeeInUSD = mosaicoFeeInUSD
            };
            paymentTransaction.SetStatus(status);
            paymentTransaction.SetType(type);

            _walletDbContext.Transactions.Add(paymentTransaction);
            await _walletDbContext.SaveChangesAsync();
            
            _logger?.Verbose($"New transaction was created");
            _logger?.Verbose($"Publishing events");
            await PublishEventAsync(paymentTransaction.Id, userId);
        }
        
        private async Task PublishEventAsync(Guid id, string userId)
        {
            var cloudEvent = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new MosaicoWalletPurchaseInitiated(id, userId, Guid.Empty));
            await _eventPublisher.PublishAsync(cloudEvent);
        }
    }
}