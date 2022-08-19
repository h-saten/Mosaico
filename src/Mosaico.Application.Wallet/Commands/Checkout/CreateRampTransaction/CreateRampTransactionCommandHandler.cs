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
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.Checkout.CreateRampTransaction
{
    public class CreateRampTransactionCommandHandler : IRequestHandler<CreateRampTransactionCommand>
    {
        private readonly ILogger _logger;
        private readonly IWalletDbContext _walletContext;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IProjectWalletService _projectWalletService;
        private readonly IFeeService _feeService;

        public CreateRampTransactionCommandHandler(IProjectManagementClient projectManagementClient, ICurrentUserContext currentUserContext, 
            IWalletDbContext walletContext, ILogger logger, IExchangeRateService exchangeRateService, IProjectWalletService projectWalletService, IFeeService feeService)
        {
            _projectManagementClient = projectManagementClient;
            _currentUserContext = currentUserContext;
            _walletContext = walletContext;
            _logger = logger;
            _exchangeRateService = exchangeRateService;
            _projectWalletService = projectWalletService;
            _feeService = feeService;
        }

        public async Task<Unit> Handle(CreateRampTransactionCommand request, CancellationToken cancellationToken)
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
                
                var correlationId = request.Id.ToLowerInvariant().Trim();
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
                
                var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(request.FiatCurrency);
                if (exchangeRate == null)
                {
                    throw new InvalidExchangeRateException(request.FiatCurrency);
                }

                var estimatedTokenAmount = ((request.FiatValue * exchangeRate.Rate) / currentSaleStage.TokenPrice);
                var tokenAmountDifference = request.TokenAmount - estimatedTokenAmount;
                if (tokenAmountDifference > 0.1m)
                {
                    request.TokenAmount = estimatedTokenAmount;
                }
                
                if (_walletContext.Transactions.Any(t => t.CorrelationId == correlationId))
                {
                    throw new TransactionAlreadyExistsException(request.Id);
                }

                var paymentCurrency = await _walletContext
                    .PaymentCurrencies
                    .Where(c => c.ContractAddress == request.Asset.Address && c.Chain == token.Network)
                    .SingleOrDefaultAsync(cancellationToken);
                
                if (paymentCurrency is null)
                {
                    throw new UnsupportedCurrencyException(request.Asset.Symbol);
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
                
                var projectWallet =
                    await _walletContext.ProjectWallets.FirstOrDefaultAsync(w =>
                        w.ProjectId == project.Id && w.Network == token.Network, cancellationToken: cancellationToken);
            
                if(projectWallet == null)
                {
                    projectWallet = await _projectWalletService.CreateWalletAsync(token.Network, project.Id);
                }

                var fee = await _feeService.GetFeeAsync(project.Id, currentSaleStage.Id);
                
                var account = await _projectWalletService.GetAccountAsync(token.Network, project.Id, _currentUserContext.UserId);

                var paymentTransaction = new Transaction
                {
                    CorrelationId = correlationId,
                    UserId = _currentUserContext.UserId,
                    TokenAmount = request.TokenAmount,
                    TokenId = token.Id,
                    PaymentProcessor = Payments.RampNetwork.Constants.PaymentProcessorName,
                    InitiatedAt = DateTimeOffset.UtcNow,
                    WalletAddress = wallet.AccountAddress,
                    Network = token.Network,
                    PaymentCurrencyId = paymentCurrency.Id,
                    Currency = request.FiatCurrency,
                    PayedAmount = request.FiatValue,
                    PaymentCurrency = paymentCurrency,
                    From = companyWallet.AccountAddress,
                    To = wallet.AccountAddress,
                    StageId = currentSaleStage.Id,
                    ExtraData = request.Secret,
                    RefCode = request.RefCode?.ToLowerInvariant().Trim(),
                    IntermediateAddress = account.Address,
                    PaymentMethod = request.PaymentMethodType,
                    ProjectId = request.ProjectId,
                    TokenPrice = currentSaleStage.TokenPrice,
                    FeePercentage = fee
                };
            
                paymentTransaction.SetStatus(status);
                paymentTransaction.SetType(type);

                _walletContext.Transactions.Add(paymentTransaction);
                await _walletContext.SaveChangesAsync(cancellationToken);
                
                _logger?.Verbose($"New transaction was created");
                _logger?.Verbose($"Publishing events");
                await transaction.CommitAsync(cancellationToken);
                return Unit.Value;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}