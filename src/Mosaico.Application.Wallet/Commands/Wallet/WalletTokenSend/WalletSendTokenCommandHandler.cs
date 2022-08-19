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

namespace Mosaico.Application.Wallet.Commands.Wallet.WalletTokenSend
{
    public class WalletSendTokenCommandHandler : IRequestHandler<WalletSendTokenCommand>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ICurrentUserContext _currentUser;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IDateTimeProvider _timeProvider;
        private readonly ITokenLockService _lockService;
        private readonly IUserWalletService _userWalletService;
        private readonly IOperationService _operationService;
        
        public WalletSendTokenCommandHandler(IWalletDbContext walletDbContext, ICurrentUserContext currentUser, IEventFactory eventFactory, IEventPublisher eventPublisher, IDateTimeProvider timeProvider, IOperationService operationService, ITokenLockService lockService, IUserWalletService userWalletService)
        {
            _walletDbContext = walletDbContext;
            _currentUser = currentUser;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _timeProvider = timeProvider;
            _operationService = operationService;
            _lockService = lockService;
            _userWalletService = userWalletService;
        }

        public async Task<Unit> Handle(WalletSendTokenCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletDbContext.Wallets.Include(t => t.Tokens).FirstOrDefaultAsync(c =>
                _currentUser.UserId == c.UserId && c.Network == request.Network, cancellationToken: cancellationToken);
            if (wallet == null)
            {
                throw new WalletNotFoundException(request.WalletAddress, request.Network);
            }

            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == request.TokenId, cancellationToken: cancellationToken);
            if (token == null)
            {
                throw new InsufficientFundsException(wallet.AccountAddress);
            }

            var contractAddress = token.Address;
            

            if (string.IsNullOrWhiteSpace(contractAddress))
            {
                throw new TokenNotFoundException($"Token not found");
            }

            var balanceResponse = await _userWalletService.GetTokenBalanceAsync(wallet, token, cancellationToken);
            if (request.Amount > balanceResponse.Balance)
            {
                throw new InsufficientFundsException(wallet.AccountAddress);
            }
            
            var transactionStatus =
                await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(t =>
                    t.Key == Domain.Wallet.Constants.TransactionStatuses.Pending, cancellationToken);

            var transactionType =
                await _walletDbContext.TransactionType.FirstOrDefaultAsync(
                    t => t.Key == Domain.Wallet.Constants.TransactionType.Transfer, cancellationToken);
            
            var operation = await _operationService.CreateTransferOperationAsync(_currentUser.UserId, token.Network, token.Address, token.Symbol);
            var tokenLock = await _lockService.CreateTokenLockAsync(token.Id, _currentUser.UserId, request.Amount, Constants.LockReasons.TRANSFER, token: cancellationToken);
            
            try
            {
                var correlationId = Guid.NewGuid();
                var transaction = new Transaction
                {
                    From = wallet.AccountAddress,
                    To = request.Address,
                    Status = transactionStatus,
                    StatusId = transactionStatus.Id,
                    Network = request.Network,
                    Type = transactionType,
                    TypeId = transactionType.Id,
                    CorrelationId = correlationId.ToString(),
                    PaymentProcessor = Constants.PaymentProcessors.Mosaico,
                    TokenAmount = request.Amount,
                    TokenId = request.TokenId,
                    WalletAddress = wallet.AccountAddress,
                    InitiatedAt = _timeProvider.Now(),
                    UserId = _currentUser.UserId
                };
                _walletDbContext.Transactions.Add(transaction);
                await _walletDbContext.SaveChangesAsync(cancellationToken);
                await PublishEventAsync(transaction.Id, operation.Id);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                await _lockService.SetExpiredAsync(tokenLock, cancellationToken);
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