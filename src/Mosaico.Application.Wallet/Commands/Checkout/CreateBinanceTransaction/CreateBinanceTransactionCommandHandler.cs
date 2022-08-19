using System;
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
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Payments.Binance.Abstractions;
using Mosaico.Payments.Binance.Configurations;
using Mosaico.Payments.Binance.Models;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.Checkout.CreateBinanceTransaction
{
    public class CreateBinanceTransactionCommandHandler : IRequestHandler<CreateBinanceTransactionCommand, CreateBinanceTransactionCommandResponse>
    {
        private readonly IWalletDbContext _walletContext;
        private readonly IBinanceClient _binanceClient;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ILogger _logger;
        private readonly IDateTimeProvider _provider;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ICrowdsalePurchaseService _crowdsalePurchaseService;
        private readonly IFeeService _feeService;
        private readonly BinanceConfiguration _binanceConfiguration;

        public CreateBinanceTransactionCommandHandler(IProjectManagementClient projectManagementClient, IBinanceClient binanceClient, ILogger logger, IFeeService feeService, ICurrentUserContext currentUserContext,  IDateTimeProvider provider, IExchangeRateService exchangeRateService, IWalletDbContext walletContext, ICrowdsalePurchaseService crowdsalePurchaseService, BinanceConfiguration binanceConfiguration)
        {
            _projectManagementClient = projectManagementClient;
            _binanceClient = binanceClient;
            _logger = logger;
            _feeService = feeService;
            _currentUserContext = currentUserContext;
            _provider = provider;
            _exchangeRateService = exchangeRateService;
            _walletContext = walletContext;
            _crowdsalePurchaseService = crowdsalePurchaseService;
            _binanceConfiguration = binanceConfiguration;
        }

        public async Task<CreateBinanceTransactionCommandResponse> Handle(CreateBinanceTransactionCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to initiate purchase transaction");
            var project = await _projectManagementClient.GetProjectDetailsAsync(request.ProjectId, cancellationToken);
            _logger?.Verbose($"Project was found");
                
            if (project == null)
                throw new ProjectNotFoundException(request.ProjectId);

            if (project.SaleInProgress is false)
                throw new InvalidProjectStatusException(request.ProjectId.ToString());
            
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

            if (!Payments.Binance.Constants.Currencies.All.Contains(request.Currency))
            {
                throw new Payments.Binance.Exceptions.UnsupportedCurrencyException(request.Currency);
            }
            
            var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(request.Currency);
            if (exchangeRate == null)
            {
                throw new InvalidExchangeRateException(request.Currency);
            }

            var payedInCurrency = request.PayedAmount;
            var paymentAmountInUSD = request.PayedAmount * exchangeRate.Rate;
            var tokenAmount = Math.Round(paymentAmountInUSD / currentSaleStage.TokenPrice, 4);
            
            var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w =>
                w.UserId == _currentUserContext.UserId
                && w.Network == token.Network, cancellationToken);
            
            if (wallet == null)
            {
                throw new WalletNotFoundException(_currentUserContext.UserId);
            }
            
            var canPurchase = await _crowdsalePurchaseService.CanPurchaseAsync(_currentUserContext.UserId, tokenAmount, currentSaleStage.Id, 
                Domain.ProjectManagement.Constants.PaymentMethods.Binance);
            if (!canPurchase)
            {
                throw new UnauthorizedPurchaseException(_currentUserContext.UserId);
            }
            
            
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

            var fee = await _feeService.GetFeeAsync(project.Id, currentSaleStage.Id);
            var mosaicoFee = _feeService.GetMosaicoFeeValueAsync(fee, 0, payedInCurrency);
            var mosaicoFeeInUSD = mosaicoFee * exchangeRate.Rate;
            
            var correlationId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 32);
            var binanceTransaction = await _binanceClient.CreateOrderAsync(new BinanceOrderCreationRequest
            {
                Currency = request.Currency,
                OrderAmount = request.PayedAmount,
                MerchantTradeNo = correlationId,
                Goods = BinanceGood.CreateDigital(token.Id, token.Symbol),
                ReturnUrl = string.Format(_binanceConfiguration.RedirectUrl, correlationId),
                CancelUrl = string.Format(_binanceConfiguration.RedirectUrl, correlationId)
            }, cancellationToken);
            
            var paymentTransaction = new Transaction
            {
                CorrelationId = correlationId,
                UserId = _currentUserContext.UserId,
                TokenAmount = tokenAmount,
                TokenId = token.Id,
                PaymentProcessor = Domain.ProjectManagement.Constants.PaymentMethods.Binance,
                InitiatedAt = _provider.Now(),
                WalletAddress = wallet.AccountAddress,
                Network = token.Network,
                Currency = request.Currency,
                PayedAmount = payedInCurrency,
                From = companyWallet.AccountAddress,
                To = wallet.AccountAddress,
                StageId = currentSaleStage.Id,
                RefCode = request.RefCode?.ToLowerInvariant().Trim(),
                ProjectId = request.ProjectId,
                PayedInUSD = paymentAmountInUSD,
                PaymentMethod = "DEFAULT",
                TokenPrice = currentSaleStage.TokenPrice,
                FeePercentage = fee,
                Fee = 0,
                FeeInUSD = 0,
                MosaicoFee = mosaicoFee,
                MosaicoFeeInUSD = mosaicoFeeInUSD,
                ExtraData = binanceTransaction.PrepayId,
                ExchangeRate = exchangeRate.Rate
            };
            paymentTransaction.SetStatus(status);
            paymentTransaction.SetType(type);

            await _walletContext.Transactions.AddAsync(paymentTransaction, cancellationToken);
            await _walletContext.SaveChangesAsync(cancellationToken);

            _logger?.Verbose($"New transaction was created");
            return new CreateBinanceTransactionCommandResponse
            {
                CheckoutUrl = binanceTransaction.CheckoutUrl
            };
        }
    }
}