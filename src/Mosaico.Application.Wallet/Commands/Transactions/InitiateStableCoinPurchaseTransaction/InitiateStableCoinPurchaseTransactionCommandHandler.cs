using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Permissions;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Domain.Wallet.ValueObjects;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.Transactions.InitiateStableCoinPurchaseTransaction
{
    public class InitiateStableCoinPurchaseTransactionCommandHandler : IRequestHandler<InitiateStableCoinPurchaseTransactionCommand, Guid>
    {
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
        private readonly IWalletDbContext _walletContext;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IProjectPermissionFactory _permissionFactory;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IUserWalletService _userWalletService;

        public InitiateStableCoinPurchaseTransactionCommandHandler(
            IEventFactory eventFactory,
            IEventPublisher eventPublisher, 
            ICurrentUserContext currentUserContext, 
            IWalletDbContext dbContext, 
            IProjectPermissionFactory permissionFactory, 
            IProjectManagementClient projectManagementClient, 
            IUserWalletService userWalletService, 
            ILogger logger = null)
        {
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _currentUserContext = currentUserContext;
            _walletContext = dbContext;
            _permissionFactory = permissionFactory;
            _projectManagementClient = projectManagementClient;
            _userWalletService = userWalletService;
            _logger = logger;
        }

        public async Task<Guid> Handle(InitiateStableCoinPurchaseTransactionCommand request, CancellationToken cancellationToken)
        {
            //TODO: improve balance check
            await using var transaction = _walletContext.BeginTransaction();
            try
            {
                _logger?.Verbose($"Attempting to initiate purchase transaction");
                var project = await _projectManagementClient.GetProjectDetailsAsync(request.ProjectId, cancellationToken);
                _logger?.Verbose($"Project was found");
                    
                if (project == null)
                    throw new ProjectNotFoundException(request.ProjectId);

                if (project.SaleInProgress is false)
                    throw new InvalidProjectStatusException(request.ProjectId.ToString());
                    
                var canPurchase = await _permissionFactory.GetUserAbilityToPurchaseAsync(project.Id, _currentUserContext.UserId, cancellationToken);
                if (!canPurchase)
                {
                    throw new UnauthorizedPurchaseException(_currentUserContext.UserId.ToString());
                }
                
                var correlationId = Guid.NewGuid();
                _logger?.Verbose($"New transaction correlation id {correlationId}");
                var token = await _walletContext.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId, cancellationToken);
                if (token == null)
                {
                    _logger?.Fatal($"Potential breach. User tries to buy tokens when he is unauthorized");
                    throw new TokenNotFoundException(project.TokenId.ToString());
                }
                    
                // TODO if payment currency not exist take native chain currency eg. ether for ethereum
                var currentSaleStage = await _projectManagementClient.CurrentProjectSaleStage(project.Id, cancellationToken);
                    
                if (currentSaleStage == null)
                {
                    throw new ProjectStageNotExistException(project.Id);
                }

                // TODO logic below only for ERC20 stable coins
                var paymentCurrency = await _walletContext
                    .PaymentCurrencies
                    .Where(c => c.Ticker == request.PaymentCurrency && c.Chain == request.Network && !c.NativeChainCurrency)
                    .SingleOrDefaultAsync(cancellationToken);
                
                if (paymentCurrency is null)
                {
                    throw new UnsupportedCurrencyException(request.PaymentCurrency);
                }
                
                var payerWallet = await _walletContext.Wallets.FirstOrDefaultAsync(w =>
                    w.UserId == _currentUserContext.UserId
                    && w.Network == token.Network, cancellationToken);
                request.WalletAddress = payerWallet.AccountAddress;

                var userBalanceInPaymentCurrency = await _userWalletService
                    .PaymentCurrencyBalanceAsync(payerWallet.AccountAddress, request.PaymentCurrency, request.Network);

                var purchaseAmount = request.TokenAmount * currentSaleStage.TokenPrice;

                var requiredPaymentCurrencyBalance = new Wei(purchaseAmount);
                
                if (userBalanceInPaymentCurrency <= requiredPaymentCurrencyBalance.ToDecimal())
                {
                    throw new InsufficientCurrencyBalanceException(request.PaymentCurrency);
                }
                
                _logger?.Verbose($"Calculating if there is enough tokens");
                var pendingTokens = await _walletContext.Transactions.AsNoTracking()
                    .Where(t => t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Pending && t.TokenId == token.Id)
                    .Select(t => t.TokenAmount)
                    .SumAsync(cancellationToken);

                // TODO where tokenLeft value is updated?
                // if (token.TokensLeft - (pendingTokens + request.TokenAmount) < 0)
                // {
                //     throw new InsufficientTokensException();
                // }
                    
                var type = await _walletContext.TransactionType.FirstOrDefaultAsync(t => t.Key == Domain.Wallet.Constants.TransactionType.Purchase, cancellationToken);
                if (type == null)
                {
                    throw new TransactionTypeNotFoundException(Domain.Wallet.Constants.TransactionType.Purchase);
                }

                var status = await _walletContext.TransactionStatuses.FirstOrDefaultAsync(t => t.Key == Domain.Wallet.Constants.TransactionStatuses.Pending, cancellationToken);
                if (status == null)
                {
                    throw new TransactionStatusNotExistException(Domain.Wallet.Constants.TransactionStatuses.Pending);
                }

                var companyWallet = await _walletContext.CompanyWallets.FirstOrDefaultAsync(c =>
                    c.CompanyId == project.CompanyId && c.Network == request.Network, cancellationToken: cancellationToken);
                
                if (companyWallet == null)
                {
                    throw new CompanyWalletNotFoundException(project.CompanyId.ToString());
                }

                var paymentTransaction = new Transaction
                {
                    CorrelationId = correlationId.ToString(),
                    UserId = _currentUserContext.UserId,
                    TokenAmount = request.TokenAmount,
                    TokenId = token.Id,
                    PaymentProcessor = request.PaymentProcessor,
                    InitiatedAt = DateTimeOffset.UtcNow,
                    WalletAddress = request.WalletAddress,
                    Network = request.Network,
                    PaymentCurrencyId = paymentCurrency.Id,
                    PayedAmount = requiredPaymentCurrencyBalance.ToDecimal(),
                    Currency = paymentCurrency.Ticker,
                    PaymentCurrency = paymentCurrency,
                    From = payerWallet.AccountAddress,
                    To = project.CrowdsaleContractAddress,
                    StageId = currentSaleStage.Id
                };
            
                paymentTransaction.SetStatus(status);
                paymentTransaction.SetType(type);

                _walletContext.Transactions.Add(paymentTransaction);
                await _walletContext.SaveChangesAsync(cancellationToken);

                _logger?.Verbose($"New transaction was created");
                _logger?.Verbose($"Publishing events");
                await transaction.CommitAsync(cancellationToken);
                await PublishEventsAsync(request, project.Id, paymentTransaction.Id, cancellationToken);
                return paymentTransaction.Id;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private async Task PublishEventsAsync(InitiateStableCoinPurchaseTransactionCommand request, Guid projectId, Guid transactionId, CancellationToken cancellationToken)
        {
            var cloudEvent = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new TransactionInitiatedEvent
                {
                    ProjectId = projectId,
                    TokenAmount = request.TokenAmount,
                    TransactionId = transactionId,
                    PaymentProcessor = request.PaymentProcessor,
                    WalletAddress = request.WalletAddress
                });
            await _eventPublisher.PublishAsync(cloudEvent.Source, cloudEvent, cancellationToken);
        }
    }
}