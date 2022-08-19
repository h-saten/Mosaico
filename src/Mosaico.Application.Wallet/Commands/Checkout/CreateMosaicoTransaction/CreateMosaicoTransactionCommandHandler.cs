using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Base.Tools;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.Checkout.CreateMosaicoTransaction
{
    public class CreateMosaicoTransactionCommandHandler : IRequestHandler<CreateMosaicoTransactionCommand, Guid>
    {
        private readonly IWalletDbContext _walletContext;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ICrowdsalePurchaseService _crowdsalePurchaseService;
        private readonly ILogger _logger;
        private readonly IDateTimeProvider _provider;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IProjectWalletService _projectWalletService;
        private readonly IUserWalletService _userWalletService;
        private readonly IFeeService _feeService;
        private readonly IOperationService _operationService;

        public CreateMosaicoTransactionCommandHandler(IWalletDbContext walletDbContext, IProjectManagementClient projectManagementClient, 
            IExchangeRateService exchangeRateService, ICrowdsalePurchaseService crowdsalePurchaseService, 
            ILogger logger, IDateTimeProvider provider, IEventPublisher eventPublisher, IEventFactory eventFactory, 
            ICurrentUserContext currentUserContext, IProjectWalletService projectWalletService, IUserWalletService userWalletService, IFeeService feeService, IOperationService operationService)
        {
            _walletContext = walletDbContext;
            _projectManagementClient = projectManagementClient;
            _exchangeRateService = exchangeRateService;
            _crowdsalePurchaseService = crowdsalePurchaseService;
            _logger = logger;
            _provider = provider;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _currentUserContext = currentUserContext;
            _projectWalletService = projectWalletService;
            _userWalletService = userWalletService;
            _feeService = feeService;
            _operationService = operationService;
        }

        public async Task<Guid> Handle(CreateMosaicoTransactionCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to initiate purchase transaction");
            var project = await _projectManagementClient.GetProjectDetailsAsync(request.ProjectId, cancellationToken);
            _logger?.Verbose($"Project was found");
                
            if (project == null)
                throw new ProjectNotFoundException(request.ProjectId);

            if (project.SaleInProgress is false)
                throw new InvalidProjectStatusException(request.ProjectId.ToString());
            
            var correlationId = Guid.NewGuid().ToString().ToLowerInvariant().Trim();
            _logger?.Verbose($"New transaction correlation id {correlationId}");
            var token = await _walletContext.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId, cancellationToken);
            if (token == null)
            {
                _logger?.Fatal($"Potential breach. User tries to buy tokens when he is unauthorized");
                throw new TokenNotFoundException(project.TokenId.ToString());
            }
            
            var currentSaleStage = await _projectManagementClient.CurrentProjectSaleStage(project.Id, cancellationToken);
            if (currentSaleStage == null)
            {
                throw new ProjectStageNotExistException(project.Id);
            }
            var currency = await _walletContext
                .PaymentCurrencies
                .FirstOrDefaultAsync(x => x.Chain == token.Network && x.Ticker == request.Currency, cancellationToken);
            
            if (currency == null) 
                throw new UnsupportedCurrencyException(request.Currency);
            
            var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(request.Currency);
            if (exchangeRate == null)
            {
                throw new InvalidExchangeRateException(request.Currency);
            }
            
            if (_walletContext.Transactions.Any(t => t.CorrelationId == correlationId))
            {
                throw new TransactionAlreadyExistsException(correlationId);
            }

            var payedInCurrency = request.PayedAmount;
            var paymentAmountInUSD = request.PayedAmount * exchangeRate.Rate;
            var tokenAmount = Math.Round(paymentAmountInUSD / currentSaleStage.TokenPrice, 2);
            
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w =>
                w.UserId == _currentUserContext.UserId
                && w.Network == token.Network, cancellationToken);
            
            if (wallet == null)
            {
                throw new WalletNotFoundException(_currentUserContext.UserId);
            }
            
            var canPurchase = await _crowdsalePurchaseService.CanPurchaseAsync(_currentUserContext.UserId, tokenAmount, currentSaleStage.Id, 
                Domain.ProjectManagement.Constants.PaymentMethods.MosaicoWallet);
            if (!canPurchase)
            {
                throw new UnauthorizedPurchaseException(_currentUserContext.UserId);
            }

            var operation = await _walletContext.Operations.FirstOrDefaultAsync(o => o.Network == token.Network &&
                o.Type == BlockchainOperationType.PURCHASE && o.State == OperationState.IN_PROGRESS
                && o.AccountAddress == wallet.AccountAddress, cancellationToken: cancellationToken);
            if (operation != null)
            {
                throw new AnotherPurchaseOperationInProgressException(_currentUserContext.UserId, project.Id);
            }
            
            operation = await _operationService.CreatePurchaseOperationAsync(_currentUserContext.UserId, token.Network, wallet.AccountAddress, project.Id);

            try
            {
                var type = await _walletContext.TransactionType.FirstOrDefaultAsync(
                    t => t.Key == Domain.Wallet.Constants.TransactionType.Purchase, cancellationToken);
                if (type == null)
                {
                    throw new TransactionTypeNotFoundException(Domain.Wallet.Constants.TransactionType.Purchase);
                }

                var status = await _walletContext.TransactionStatuses.FirstOrDefaultAsync(
                    t => t.Key == Domain.Wallet.Constants.TransactionStatuses.Pending, cancellationToken);
                if (status == null)
                {
                    throw new TransactionStatusNotExistException(Domain.Wallet.Constants.TransactionStatuses.Pending);
                }

                var companyWallet = await _walletContext.CompanyWallets.FirstOrDefaultAsync(c =>
                        c.CompanyId == project.CompanyId && c.Network == token.Network,
                    cancellationToken: cancellationToken);

                if (companyWallet == null)
                {
                    throw new CompanyWalletNotFoundException(project.CompanyId.ToString());
                }

                var walletBalance = currency.NativeChainCurrency
                    ? await _userWalletService.NativePaymentCurrencyBalanceAsync(wallet.AccountAddress, currency.Ticker,
                        wallet.Network)
                    : await _userWalletService.PaymentCurrencyBalanceAsync(wallet.AccountAddress, currency.Ticker,
                        wallet.Network);

                if (walletBalance < request.PayedAmount) throw new InsufficientFundsException(wallet.AccountAddress);

                var projectWallet =
                    await _walletContext.ProjectWallets.FirstOrDefaultAsync(w =>
                        w.ProjectId == project.Id && w.Network == token.Network, cancellationToken: cancellationToken);
                
                if (projectWallet == null)
                {
                    projectWallet = await _projectWalletService.CreateWalletAsync(token.Network, project.Id);
                }

                var fee = await _feeService.GetFeeAsync(project.Id, currentSaleStage.Id);
                var mosaicoFee = _feeService.GetMosaicoFeeValueAsync(fee, 0, payedInCurrency);
                var mosaicoFeeInUSD = mosaicoFee * exchangeRate.Rate;
                var account = await _projectWalletService.GetAccountAsync(token.Network, project.Id, _currentUserContext.UserId);
                
                var paymentTransaction = new Transaction
                {
                    CorrelationId = correlationId,
                    UserId = _currentUserContext.UserId,
                    TokenAmount = tokenAmount,
                    TokenId = token.Id,
                    PaymentProcessor = Domain.ProjectManagement.Constants.PaymentMethods.MosaicoWallet,
                    InitiatedAt = _provider.Now(),
                    WalletAddress = wallet.AccountAddress,
                    Network = token.Network,
                    PaymentCurrencyId = currency.Id,
                    Currency = request.Currency,
                    PayedAmount = payedInCurrency,
                    PaymentCurrency = currency,
                    From = companyWallet.AccountAddress,
                    To = wallet.AccountAddress,
                    StageId = currentSaleStage.Id,
                    RefCode = request.RefCode?.ToLowerInvariant().Trim(),
                    ProjectId = request.ProjectId,
                    PayedInUSD = paymentAmountInUSD,
                    IntermediateAddress = account.Address,
                    PaymentMethod = "DEFAULT",
                    TokenPrice = currentSaleStage.TokenPrice,
                    FeePercentage = fee,
                    Fee = 0,
                    FeeInUSD = 0,
                    MosaicoFee = mosaicoFee,
                    MosaicoFeeInUSD = mosaicoFeeInUSD,
                    ExchangeRate = exchangeRate.Rate
                };
                paymentTransaction.SetStatus(status);
                paymentTransaction.SetType(type);

                await _walletContext.Transactions.AddAsync(paymentTransaction, cancellationToken);
                await _walletContext.SaveChangesAsync(cancellationToken);

                await _operationService.SetTransactionInProgress(operation.Id, null, paymentTransaction.Id);

                _logger?.Verbose($"New transaction was created");
                _logger?.Verbose($"Publishing events");
                await PublishEventAsync(paymentTransaction.Id, _currentUserContext.UserId, operation.Id);
                return paymentTransaction.Id;
            }
            catch (Exception ex)
            {
                await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                _logger?.Error(ex, $"Error during purchase of the project {project.Id} by the user {_currentUserContext.UserId}");
                throw;
            }
        }
        
        private async Task PublishEventAsync(Guid id, string userId, Guid operationId)
        {
            var cloudEvent = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new MosaicoWalletPurchaseInitiated(id, userId, operationId));
            await _eventPublisher.PublishAsync(cloudEvent);
        }
    }
}