using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Base.Tools;
using Mosaico.Blockchain.Base.DAL;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Domain.Wallet.ValueObjects;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.SDK.Identity.Abstractions;

namespace Mosaico.Application.Wallet.Commands.Wallet.CompanyWalletCurrencySend
{
    public class CompanyWalletSendCurrencyCommandHandler : IRequestHandler<CompanyWalletSendCurrencyCommand>
    {
        private readonly IUserManagementClient _managementClient; 
        private readonly IWalletDbContext _walletDbContext;
        private readonly ICurrentUserContext _currentUser;
        private readonly IAccountRepository _accountRepository;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IOperationService _operationService;

        public CompanyWalletSendCurrencyCommandHandler(IUserManagementClient managementClient, IWalletDbContext walletDbContext, ICurrentUserContext currentUser, IAccountRepository accountRepository, IEventFactory eventFactory, IEventPublisher eventPublisher, IDateTimeProvider timeProvider, IOperationService operationService)
        {
            _managementClient = managementClient;
            _walletDbContext = walletDbContext;
            _currentUser = currentUser;
            _accountRepository = accountRepository;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _timeProvider = timeProvider;
            _operationService = operationService;
        }

        public async Task<Unit> Handle(CompanyWalletSendCurrencyCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletDbContext.CompanyWallets.Include(t => t.Tokens).FirstOrDefaultAsync(c =>
                c.CompanyId == request.CompanyId, cancellationToken: cancellationToken);
            if (wallet == null)
            {
                throw new CompanyWalletNotFoundException(request.CompanyId.ToString());
            }

            var permissions = await _managementClient.GetUserPermissionsAsync(_currentUser.UserId, wallet.CompanyId, cancellationToken);
            if (!permissions.Any(p => p.Key == Authorization.Base.Constants.Permissions.Company.CanEditDetails))
            {
                throw new ForbiddenException();
            }
            
            var paymentCurrency = await _walletDbContext
                .PaymentCurrencies
                .FirstOrDefaultAsync(p => p.Id == request.PaymentCurrencyId && p.Chain == wallet.Network, cancellationToken);
            
            if (paymentCurrency == null)
            {
                throw new TokenNotFoundException($"Token not found");
            }

            decimal balance;
            if (paymentCurrency.NativeChainCurrency)
            {
                var nativeBalanceResponse = await _accountRepository.AccountBalanceAsync(wallet.AccountAddress, wallet.Network);
                balance = nativeBalanceResponse.Balance;
            }
            else
            {
                var balanceResponse = await _accountRepository.Erc20BalanceAsync(wallet.AccountAddress, paymentCurrency.ContractAddress, wallet.Network);
                balance = new Wei(balanceResponse, paymentCurrency.DecimalPlaces).ToDecimal();
            }

            if (request.Amount > balance)
            {
                throw new InsufficientFundsException(wallet.AccountAddress);
            }

            var transactionStatus =
                await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(t =>
                    t.Key == Domain.Wallet.Constants.TransactionStatuses.Pending, cancellationToken);

            var transactionType =
                await _walletDbContext.TransactionType.FirstOrDefaultAsync(
                    t => t.Key == Domain.Wallet.Constants.TransactionType.Transfer, cancellationToken);

            var operation = await _operationService.CreateDaoTransferOperationAsync(wallet.CompanyId, _currentUser.UserId, paymentCurrency.Chain, paymentCurrency.ContractAddress, paymentCurrency.Ticker);
            var correlationId = Guid.NewGuid();
            var transaction = new Transaction
            {
                From = wallet.AccountAddress,
                To = request.Address,
                Status = transactionStatus,
                StatusId = transactionStatus.Id,
                Network = wallet.Network,
                Type = transactionType,
                TypeId = transactionType.Id,
                CorrelationId = correlationId.ToString(),
                PaymentProcessor = Constants.PaymentProcessors.Mosaico,
                TokenAmount = request.Amount,
                PaymentCurrencyId = paymentCurrency.Id,
                PaymentCurrency = paymentCurrency,
                InitiatedAt = _timeProvider.Now(),
                WalletAddress = wallet.AccountAddress,
                UserId = _currentUser.UserId
            };
            _walletDbContext.Transactions.Add(transaction);
            await _walletDbContext.SaveChangesAsync(cancellationToken);
            await PublishEventAsync(transaction.Id, operation.Id);
            return Unit.Value;
        }
        
        private async Task PublishEventAsync(Guid transactionId, Guid operationId)
        {
            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new NonCustodialTransactionInitiated(transactionId, operationId));
            await _eventPublisher.PublishAsync(e);
        }
    }
}