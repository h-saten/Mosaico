using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Base.Tools;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.Checkout.CreateBankTransferTransaction
{
    public class CreateBankTransferTransactionCommandHandler : IRequestHandler<CreateBankTransferTransactionCommand, ProjectBankPaymentDetailsDTO>
    {
        private readonly IBankTransferReferenceService _transferReference;
        private readonly IWalletDbContext _walletDbContext;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ICrowdsalePurchaseService _crowdsalePurchaseService;
        private readonly IUserManagementClient _userManagementClient;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IDateTimeProvider _provider;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IFeeService _feeService;

        public CreateBankTransferTransactionCommandHandler(IBankTransferReferenceService transferReference, IWalletDbContext walletDbContext, IMapper mapper, IProjectManagementClient projectManagementClient, IExchangeRateService exchangeRateService, ICrowdsalePurchaseService crowdsalePurchaseService, ILogger logger, IDateTimeProvider provider, IEventPublisher eventPublisher, IEventFactory eventFactory, IUserManagementClient userManagementClient, IFeeService feeService)
        {
            _transferReference = transferReference;
            _walletDbContext = walletDbContext;
            _mapper = mapper;
            _projectManagementClient = projectManagementClient;
            _exchangeRateService = exchangeRateService;
            _crowdsalePurchaseService = crowdsalePurchaseService;
            _logger = logger;
            _provider = provider;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _userManagementClient = userManagementClient;
            _feeService = feeService;
        }

        public async Task<ProjectBankPaymentDetailsDTO> Handle(CreateBankTransferTransactionCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectManagementClient.GetProjectDetailsAsync(request.ProjectId, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            var user = await _userManagementClient.GetUserAsync(request.UserId, cancellationToken);
            if (!user.IsAMLVerified)
            {
                throw new UserNotVerifiedException(request.UserId);
            }
            
            var paymentDetails =
                await _walletDbContext.ProjectBankPaymentDetails.FirstOrDefaultAsync(pd => pd.ProjectId == request.ProjectId, 
                    cancellationToken);
            if (paymentDetails == null)
            {
                throw new PaymentDetailsNotFoundException(project.Id);
            }
            
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId, cancellationToken);
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
            
            var canPurchase = await _crowdsalePurchaseService.CanPurchaseAsync(request.UserId, request.TokenAmount, currentSaleStage.Id, 
                Domain.ProjectManagement.Constants.PaymentMethods.BankTransfer);
            if (!canPurchase)
            {
                throw new UnauthorizedPurchaseException(request.UserId);
            }

            var bankPaymentDetails = _mapper.Map<ProjectBankPaymentDetailsDTO>(paymentDetails);
            bankPaymentDetails.Reference = await _transferReference.GenerateReferenceAsync(paymentDetails, request.Currency, request.TokenAmount, request.FiatAmount, 
                request.UserId);
            
            var type = await _walletDbContext.TransactionType.FirstOrDefaultAsync(t => t.Key == Domain.Wallet.Constants.TransactionType.Purchase, cancellationToken);
            if (type == null)
            {
                throw new TransactionTypeNotFoundException(Domain.Wallet.Constants.TransactionType.Purchase);
            }

            var status = await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(t => t.Key == Domain.Wallet.Constants.TransactionStatuses.Pending, cancellationToken);
            if (status == null)
            {
                throw new TransactionStatusNotExistException(Domain.Wallet.Constants.TransactionStatuses.Pending);
            }

            var companyWallet = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(c =>
                c.CompanyId == project.CompanyId && c.Network == token.Network, cancellationToken: cancellationToken);
            
            if (companyWallet == null)
            {
                throw new CompanyWalletNotFoundException(project.CompanyId.ToString());
            }

            var wallet = await _walletDbContext.Wallets.FirstOrDefaultAsync(w =>
                w.UserId == request.UserId
                && w.Network == token.Network, cancellationToken);
            
            if (wallet == null)
            {
                throw new WalletNotFoundException(request.UserId);
            }
            var fee = await _feeService.GetFeeAsync(project.Id, currentSaleStage.Id);
            var rates = await _exchangeRateService.GetExchangeRatesAsync();
            
            var paymentTransaction = new Transaction
            {
                CorrelationId = bankPaymentDetails.Reference.ToLowerInvariant().Trim(),
                UserId = request.UserId,
                TokenAmount = request.TokenAmount,
                TokenId = token.Id,
                PaymentProcessor = Domain.ProjectManagement.Constants.PaymentMethods.BankTransfer,
                InitiatedAt = _provider.Now(),
                WalletAddress = wallet.AccountAddress,
                Network = token.Network,
                Currency = request.Currency,
                PayedAmount = request.FiatAmount,
                From = companyWallet.AccountAddress,
                To = wallet.AccountAddress,
                StageId = currentSaleStage.Id,
                PaymentMethod = "BANK_TRANSFER",
                RefCode = request.RefCode?.ToLowerInvariant().Trim(),
                ProjectId = request.ProjectId,
                TokenPrice = currentSaleStage.TokenPrice,
                FeePercentage = fee
            };
            var rate = rates.FirstOrDefault(r => r.Ticker == paymentTransaction.Currency);
            paymentTransaction.ExchangeRate = rate?.Rate ?? 0;
            paymentTransaction.PayedInUSD = paymentTransaction.PayedAmount * paymentTransaction.ExchangeRate;
            paymentTransaction.Fee = 0;
            paymentTransaction.FeeInUSD = 0;
            paymentTransaction.MosaicoFee = _feeService.GetMosaicoFeeValueAsync(paymentTransaction.FeePercentage, paymentTransaction.Fee, paymentTransaction.PayedAmount.Value);
            paymentTransaction.MosaicoFeeInUSD = paymentTransaction.MosaicoFee * paymentTransaction.ExchangeRate;
        
            paymentTransaction.SetStatus(status);
            paymentTransaction.SetType(type);

            _walletDbContext.Transactions.Add(paymentTransaction);
            await _walletDbContext.SaveChangesAsync(cancellationToken);
            await PublishMessageAsync(paymentTransaction.UserId, paymentTransaction.Id);
            _logger?.Verbose($"New transaction was created");
            _logger?.Verbose($"Publishing events");
            
            return bankPaymentDetails;
        }
        
        private async Task PublishMessageAsync(string userId, Guid transactionId)
        {
            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new BankTransferTransactionInitiatedEvent(userId, transactionId));
            await _eventPublisher.PublishAsync(e);
        }
    }
}