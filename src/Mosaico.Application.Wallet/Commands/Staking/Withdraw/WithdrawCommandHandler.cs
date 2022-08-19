using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Abstractions;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.Staking.Withdraw
{
    public class WithdrawCommandHandler : IRequestHandler<WithdrawCommand>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IOperationService _operationService;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;

        public WithdrawCommandHandler(IWalletDbContext walletDbContext, IOperationService operationService, IEventFactory eventFactory, IEventPublisher eventPublisher, ILogger logger)
        {
            _walletDbContext = walletDbContext;
            _operationService = operationService;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<Unit> Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            var pair = await _walletDbContext.StakingPairs.FirstOrDefaultAsync(
                t => t.Id == request.Id, cancellationToken);
            
            if(pair == null)
            {
                throw new StakingPairNotFoundException(request.Id);
            }

            if (pair.IsWithdrawalDisabled)
            {
                throw new StakingIsDisabledException(pair.Id);
            }

            var stakes = await _walletDbContext.Stakings.Where(s =>
                s.Status == StakingStatus.Deployed && s.UserId == request.UserId && s.StakingPairId == pair.Id && s.WalletType == StakingWallet.MOSAICO_WALLET).ToListAsync(cancellationToken);
            if (!stakes.Any())
            {
                throw new StakingNotFoundException(pair.Id);
            }

            var operation = await _walletDbContext.Operations
                .FirstOrDefaultAsync(t => t.Network == pair.Network && t.Type == BlockchainOperationType.UNSTAKING &&
                                          t.State == OperationState.IN_PROGRESS
                                          && t.UserId == request.UserId, cancellationToken);
            if (operation != null)
            {
                throw new AnotherStakingInProgressException(request.UserId);
            }
            
            operation = await _operationService.CreateStakeWithdrawalOperationAsync(pair.Network, request.UserId, pair.Id);
            try
            {
                var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                    new WithdrawStake(pair.Id, operation.Id, request.UserId));
                await _eventPublisher.PublishAsync(e);
                return Unit.Value;
            }
            catch(Exception ex)
            {
                _logger?.Error(ex, "Error during withdrawal");
                await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                throw;
            }
        }
    }
}