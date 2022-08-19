using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;

namespace Mosaico.Application.Wallet.Commands.Staking.Stake
{
    public class StakeCommandHandler : IRequestHandler<StakeCommand>
    {
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWalletDbContext _context;
        private readonly ICurrentUserContext _currentUser;
        private readonly ITokenLockService _lockService;
        private readonly IOperationService _operationService;
        private readonly IUserWalletService _userWalletService;

        public StakeCommandHandler(IEventFactory eventFactory, IEventPublisher eventPublisher, IWalletDbContext context, ICurrentUserContext currentUser, IOperationService operationService, ITokenLockService lockService, IUserWalletService userWalletService)
        {
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _context = context;
            _currentUser = currentUser;
            _operationService = operationService;
            _lockService = lockService;
            _userWalletService = userWalletService;
        }

        public async Task<Unit> Handle(StakeCommand request, CancellationToken cancellationToken)
        {
            var pair = await _context.StakingPairs.FirstOrDefaultAsync(
                t => t.Id == request.StakingPairId, cancellationToken);
            if(pair == null)
            {
                throw new StakingPairNotFoundException(request.StakingPairId);
            }
            
            if (!pair.IsEnabled)
            {
                throw new StakingIsDisabledException(pair.Id);
            }

            var userWallet =
                await _context.Wallets.FirstOrDefaultAsync(t =>
                    t.UserId == _currentUser.UserId && t.Network == pair.Network, cancellationToken);
            
            if (userWallet == null) throw new WalletNotFoundException(_currentUser.UserId);

            var balance = pair.Type == StakingPairBaseCurrencyType.Currency
                ? await _userWalletService.GetCurrencyBalanceAsync(userWallet, pair.StakingPaymentCurrency.Ticker,
                    pair.StakingPaymentCurrency.Chain)
                : await _userWalletService.GetTokenBalanceAsync(userWallet, pair.StakingToken, CancellationToken.None);
            
            if (request.Balance > balance.Balance)
            {
                throw new InsufficientFundsException(userWallet.AccountAddress);
            }
            
            var operation = await _context.Operations
                .FirstOrDefaultAsync(t => t.Network == pair.Network && t.Type == BlockchainOperationType.STAKING &&
                                          t.State == OperationState.IN_PROGRESS && t.TransactionId == pair.Id
                                          && t.UserId == _currentUser.UserId, cancellationToken);
            if (operation != null)
            {
                throw new AnotherStakingInProgressException(_currentUser.UserId);
            }
            
            operation = await _operationService.CreateStakingOperationAsync(pair.Network, _currentUser.UserId, pair.Id);
            
            var tokenLock = pair.Type == StakingPairBaseCurrencyType.Currency
                ? await _lockService.CreateCurrencyLockAsync(pair.StakingPaymentCurrencyId.Value, _currentUser.UserId,
                    request.Balance, Constants.LockReasons.STAKE, token: cancellationToken) :
                await _lockService.CreateTokenLockAsync(pair.StakingTokenId.Value, _currentUser.UserId,
                    request.Balance, Constants.LockReasons.STAKE, token: cancellationToken) ;
            try
            {
                var staking = new Domain.Wallet.Entities.Staking.Staking
                {
                    Balance = request.Balance,
                    Days = request.Days,
                    Status = StakingStatus.Pending,
                    StakingPairId = pair.Id,
                    StakingPair = pair,
                    UserId = _currentUser.UserId
                };
                _context.Stakings.Add(staking);
                await _context.SaveChangesAsync(cancellationToken);
                await PublishEventsAsync(staking.Id, operation.Id);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                if (tokenLock != null)
                {
                    await _lockService.SetExpiredAsync(tokenLock, cancellationToken);
                }
                await _lockService.SetExpiredAsync(tokenLock, cancellationToken);
                await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                throw;
            }
        }

        private async Task PublishEventsAsync(Guid stakingId, Guid operationId)
        {
            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new StakeInitiated(stakingId, operationId));
            await _eventPublisher.PublishAsync(e);
        }
    }
}