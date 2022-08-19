using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.Checkout.CreateMetamaskTransaction
{
    public class CreateMetamaskTransactionCommandHandler : IRequestHandler<CreateMetamaskTransactionCommand, Guid>
    {
        private readonly ILogger _logger;
        private readonly IWalletDbContext _walletContext;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ICrowdsalePurchaseService _crowdsalePurchaseService;
        private readonly IFeeService _feeService;

        public CreateMetamaskTransactionCommandHandler(ILogger logger, IWalletDbContext walletContext, ICurrentUserContext currentUserContext, IProjectManagementClient projectManagementClient, IExchangeRateService exchangeRateService, ICrowdsalePurchaseService crowdsalePurchaseService, IFeeService feeService)
        {
            _logger = logger;
            _walletContext = walletContext;
            _currentUserContext = currentUserContext;
            _projectManagementClient = projectManagementClient;
            _exchangeRateService = exchangeRateService;
            _crowdsalePurchaseService = crowdsalePurchaseService;
            _feeService = feeService;
        }

        public async Task<Guid> Handle(CreateMetamaskTransactionCommand request, CancellationToken cancellationToken)
        {
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
                
                var correlationId = Guid.NewGuid().ToString();
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
                
                var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(request.Currency);
                if (exchangeRate == null)
                {
                    throw new InvalidExchangeRateException(request.Currency);
                }

                var estimatedTokenAmount = ((request.FiatAmount * exchangeRate.Rate) / currentSaleStage.TokenPrice);
                var tokenAmountDifference = request.TokenAmount - estimatedTokenAmount;
                if (tokenAmountDifference > 0.1m)
                {
                    request.TokenAmount = estimatedTokenAmount;
                }
                
                if (_walletContext.Transactions.Any(t => t.ExchangeTransactionHash == request.TransactionHash))
                {
                    throw new TransactionAlreadyExistsException(request.TransactionHash);
                }

                var paymentCurrency = await _walletContext
                    .PaymentCurrencies
                    .Where(c => c.Ticker == request.Currency && c.Chain == token.Network)
                    .SingleOrDefaultAsync(cancellationToken);
                
                if (paymentCurrency is null)
                {
                    throw new UnsupportedCurrencyException(request.Currency);
                }
                
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
                    c.CompanyId == project.CompanyId && c.Network == token.Network, cancellationToken: cancellationToken);
                
                if (companyWallet == null)
                {
                    throw new CompanyWalletNotFoundException(project.CompanyId.ToString());
                }

                var wallet = await _walletContext.Wallets.FirstOrDefaultAsync(w =>
                    w.UserId == _currentUserContext.UserId
                    && w.Network == token.Network, cancellationToken);
                
                if (wallet == null)
                {
                    throw new WalletNotFoundException(_currentUserContext.UserId);
                }

                var projectWallet = await _walletContext.ProjectWalletAccounts
                    .FirstOrDefaultAsync(w =>
                        w.ProjectWallet.ProjectId == project.Id && w.ProjectWallet.Network == wallet.Network &&
                        w.UserId == _currentUserContext.UserId, cancellationToken: cancellationToken);
                
                var fee = await _feeService.GetFeeAsync(project.Id, currentSaleStage.Id);
                
                var paymentTransaction = new Transaction
                {
                    CorrelationId = correlationId,
                    UserId = _currentUserContext.UserId,
                    TokenAmount = request.TokenAmount,
                    TokenId = token.Id,
                    PaymentProcessor = Domain.ProjectManagement.Constants.PaymentMethods.Metamask,
                    InitiatedAt = DateTimeOffset.UtcNow,
                    WalletAddress = wallet.AccountAddress,
                    Network = token.Network,
                    PaymentCurrencyId = paymentCurrency.Id,
                    ExchangeTransactionHash = request.TransactionHash,
                    Currency = request.Currency,
                    PayedAmount = request.FiatAmount,
                    PaymentCurrency = paymentCurrency,
                    From = companyWallet.AccountAddress,
                    To = wallet.AccountAddress,
                    StageId = currentSaleStage.Id,
                    RefCode = request.RefCode?.ToLowerInvariant().Trim(),
                    ProjectId = request.ProjectId,
                    IntermediateAddress = projectWallet.Address,
                    TokenPrice = currentSaleStage.TokenPrice,
                    FeePercentage = fee,
                    ExchangeRate = exchangeRate.Rate
                };
            
                paymentTransaction.SetStatus(status);
                paymentTransaction.SetType(type);

                _walletContext.Transactions.Add(paymentTransaction);
                await _walletContext.SaveChangesAsync(cancellationToken);
                
                _logger?.Verbose($"New transaction was created");
                _logger?.Verbose($"Publishing events");
                await transaction.CommitAsync(cancellationToken);
                return paymentTransaction.Id;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}