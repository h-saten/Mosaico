using System;
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

namespace Mosaico.Application.Wallet.Commands.Wallet.WalletCurrencySend
{
    public class WalletSendCurrencyCommandHandler : IRequestHandler<WalletSendCurrencyCommand>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ICurrentUserContext _currentUser;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IDateTimeProvider _provider;
        private readonly IOperationService _operationService;
        private readonly IUserWalletService _userWalletService;
        
        public WalletSendCurrencyCommandHandler(IWalletDbContext walletDbContext, ICurrentUserContext currentUser, IEventFactory eventFactory, IEventPublisher eventPublisher, IDateTimeProvider provider, IOperationService operationService, IUserWalletService userWalletService)
        {
            _walletDbContext = walletDbContext;
            _currentUser = currentUser;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _provider = provider;
            _operationService = operationService;
            _userWalletService = userWalletService;
        }

        public async Task<Unit> Handle(WalletSendCurrencyCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletDbContext.Wallets.Include(t => t.Tokens).FirstOrDefaultAsync(c =>
                c.AccountAddress == request.WalletAddress && c.Network == request.Network, cancellationToken: cancellationToken);
            
            if (wallet == null)
            {
                throw new WalletNotFoundException(request.WalletAddress, request.Network);
            }

            if (wallet.UserId != _currentUser.UserId)
            {
                throw new ForbiddenException();
            }

            var paymentCurrency = await _walletDbContext
                .PaymentCurrencies
                .FirstOrDefaultAsync(p => p.Id == request.PaymentCurrencyId && p.Chain == request.Network, cancellationToken);
            
            if (paymentCurrency == null)
            {
                throw new TokenNotFoundException($"Token not found");
            }

            var balance = await _userWalletService.GetCurrencyBalanceAsync(wallet, paymentCurrency.Ticker, paymentCurrency.Chain);
            
            if (request.Amount > balance.Balance)
            {
                throw new InsufficientFundsException(wallet.AccountAddress);
            }

            var transactionStatus =
                await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(t =>
                    t.Key == Domain.Wallet.Constants.TransactionStatuses.Pending, cancellationToken);

            var transactionType =
                await _walletDbContext.TransactionType.FirstOrDefaultAsync(
                    t => t.Key == Domain.Wallet.Constants.TransactionType.Transfer, cancellationToken);
            
            var operation = await _operationService.CreateTransferOperationAsync(_currentUser.UserId, paymentCurrency.Chain, paymentCurrency.ContractAddress, paymentCurrency.Ticker);

            var correlationId = Guid.NewGuid();
            try
            {
                var transaction = new Transaction
                {
                    From = wallet.AccountAddress,
                    To = request.Address,
                    Status = transactionStatus,
                    StatusId = transactionStatus.Id,
                    Network = paymentCurrency.Chain,
                    Type = transactionType,
                    TypeId = transactionType.Id,
                    CorrelationId = correlationId.ToString(),
                    PaymentProcessor = Constants.PaymentProcessors.Mosaico,
                    TokenAmount = request.Amount,
                    PaymentCurrencyId = paymentCurrency.Id,
                    PaymentCurrency = paymentCurrency,
                    InitiatedAt = _provider.Now(),
                    WalletAddress = wallet.AccountAddress,
                    UserId = _currentUser.UserId
                };
                _walletDbContext.Transactions.Add(transaction);
                await _walletDbContext.SaveChangesAsync(cancellationToken);
                await PublishEventAsync(transaction.Id, operation.Id);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                throw;
            }
        }
        
        private async Task PublishEventAsync(Guid transactionId, Guid operationId)
        {
            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new NonCustodialTransactionInitiated(transactionId, operationId));
            await _eventPublisher.PublishAsync(e);
        }
    }
}