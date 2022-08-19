using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KangaExchange.SDK.Abstractions;
using KangaExchange.SDK.Configurations;
using KangaExchange.SDK.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.KangaWallet.Exceptions;
using Mosaico.Application.ProjectManagement.Permissions;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;
using Constants = Mosaico.Domain.Wallet.Constants;

namespace Mosaico.Application.KangaWallet.Commands.CreateTransaction
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, KangaBuyResponseDto>
    {
        private readonly ILogger _logger;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IKangaBuyApiClient _kangaBuyApiClient;
        private readonly IUserManagementClient _userManagementClient; 
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IProjectPermissionFactory _permissionFactory;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IWalletDbContext _walletContext;
        private readonly KangaConfiguration _kangaConfiguration;
        private readonly IExchangeRateService _exchangeRateService;
        
        public CreateTransactionCommandHandler(
            IKangaBuyApiClient kangaBuyApiClient, 
            IUserManagementClient userManagementClient, 
            IProjectManagementClient projectManagementClient, 
            IProjectPermissionFactory permissionFactory, 
            ICurrentUserContext currentUserContext, 
            IWalletDbContext walletContext, 
            IEventPublisher eventPublisher, 
            IEventFactory eventFactory, 
            KangaConfiguration kangaApiConfiguration, IExchangeRateService exchangeRateService, ILogger logger = null) 
        {
            _kangaBuyApiClient = kangaBuyApiClient;
            _userManagementClient = userManagementClient;
            _projectManagementClient = projectManagementClient;
            _permissionFactory = permissionFactory;
            _currentUserContext = currentUserContext;
            _walletContext = walletContext;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _kangaConfiguration = kangaApiConfiguration;
            _exchangeRateService = exchangeRateService;
            _logger = logger;
        }
    
        public async Task<KangaBuyResponseDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserContext.UserId;
            
            var accountCreated = await _userManagementClient.CreateKangaUserIfNotExist(userId, cancellationToken);

            var project = await _projectManagementClient.GetProjectDetailsAsync(request.ProjectId, cancellationToken);
            _logger?.Verbose($"Project was found");
                    
            if (project == null)
                throw new ProjectNotFoundException(request.ProjectId);

            if (project.SaleInProgress is false)
                throw new InvalidProjectStatusException(request.ProjectId);

            if (project.PaymentMethods.Contains(Domain.ProjectManagement.Constants.PaymentMethods.KangaExchange) is false)
            {
                throw new KangaPaymentsUnsupportedException();
            }

            var canPurchase = await _permissionFactory.GetUserAbilityToPurchaseAsync(project.Id, userId, cancellationToken);
            if (!canPurchase)
            {
                throw new UnauthorizedPurchaseException(userId);
            }
            
            var token = await _walletContext.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId, cancellationToken);
            if (token == null)
            {
                _logger?.Fatal($"Potential breach. User tries to buy tokens when he is unauthorized");
                throw new TokenNotFoundException(project.TokenId.ToString());
            }
            
            var activeStage = project.Stages.FirstOrDefault(x => x.Id == project.ActiveStageId);

            var redirectAfterPaymentUrl = string.Format(_kangaConfiguration.AfterPurchaseRedirectUrl, project.Slug);
            var tokenSymbol = token.Symbol;
            if (KangaExchange.SDK.Constants.KangaMarketMappingReverse.ContainsKey(tokenSymbol))
            {
                tokenSymbol = KangaExchange.SDK.Constants.KangaMarketMappingReverse[tokenSymbol];
            }
            BuyResponseDto kangaBuyResponse;
            try
            {
                kangaBuyResponse = await _kangaBuyApiClient.BuyAsync(x =>
                {
                    x.Email = _currentUserContext.Email;
                    x.PaymentCurrency = new KangaPaymentCurrency(request.PaymentCurrency);
                    x.CurrencyAmount = request.CurrencyAmount;
                    x.TokenSymbol = tokenSymbol;
                    x.CustomRedirectUrl = redirectAfterPaymentUrl;
                    x.BuyCode = activeStage?.IsPrivate is null ? null : activeStage.AuthorizationCode;
                });
            }
            catch (Exception)
            {
                throw new KangaBuyFailedException(request.TokenAmount, token.Symbol, _currentUserContext.Email);
            }

            var type = await _walletContext.TransactionType.FirstOrDefaultAsync(t => t.Key == Constants.TransactionType.Purchase, cancellationToken);
            if (type == null)
            {
                throw new TransactionTypeNotFoundException(Constants.TransactionType.Purchase);
            }

            var status = await _walletContext.TransactionStatuses.FirstOrDefaultAsync(t => t.Key == Constants.TransactionStatuses.Pending, cancellationToken);
            if (status == null)
            {
                throw new TransactionStatusNotExistException(Constants.TransactionStatuses.Pending);
            }

            var rate = await _exchangeRateService.GetExchangeRateAsync(request.PaymentCurrency);

            var paymentTransaction = new Transaction
            {
                UserId = _currentUserContext.UserId,
                TokenAmount = request.TokenAmount,
                PayedAmount = request.CurrencyAmount,
                TokenId = token.Id,
                PaymentProcessor = Domain.ProjectManagement.Constants.PaymentMethods.KangaExchange,
                PaymentMethod = "Fiat",
                InitiatedAt = DateTimeOffset.UtcNow,
                Currency = request.PaymentCurrency,
                RefCode = request.RefCode?.ToLowerInvariant().Trim(),
                Network = token.Network,
                CorrelationId = kangaBuyResponse.OrderId,
                ExtraData = kangaBuyResponse.OrderId,
                TokenPrice = activeStage?.TokenPrice,
                ExchangeRate = rate?.Rate,
                StageId = activeStage?.Id,
                ProjectId = activeStage?.ProjectId
            };
            paymentTransaction.SetStatus(status);
            paymentTransaction.SetType(type);
            _walletContext.Transactions.Add(paymentTransaction);
            
            await _walletContext.SaveChangesAsync(cancellationToken);
            
            await PublishEventsAsync(paymentTransaction, project.Id, cancellationToken);

            var response = new KangaBuyResponseDto
            {
                RedirectUrl = kangaBuyResponse.RedirectUrl,
                KangaAccountCreated = accountCreated,
                TransactionId = paymentTransaction.Id.ToString()
            };
            
            return response;
        }

        private async Task PublishEventsAsync(Transaction transaction, Guid projectId, CancellationToken cancellationToken)
        {
            var cloudEvent = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new TransactionInitiatedEvent
                {
                    ProjectId = projectId,
                    TokenAmount = transaction.TokenAmount,
                    TransactionId = transaction.Id,
                    PaymentProcessor = transaction.PaymentProcessor,
                    WalletAddress = null
                });
            await _eventPublisher.PublishAsync(cloudEvent.Source, cloudEvent, cancellationToken);
        }
    }
}
