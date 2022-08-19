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

namespace Mosaico.Application.Wallet.Commands.Staking.Claim
{
    public class ClaimCommandHandler : IRequestHandler<ClaimCommand>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IOperationService _operationService;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWalletStakingService _walletStakingService;
        private readonly ILogger _logger;

        public ClaimCommandHandler(ILogger logger, IWalletStakingService walletStakingService, IEventPublisher eventPublisher, IOperationService operationService, IEventFactory eventFactory, IWalletDbContext walletDbContext)
        {
            _logger = logger;
            _walletStakingService = walletStakingService;
            _eventPublisher = eventPublisher;
            _operationService = operationService;
            _eventFactory = eventFactory;
            _walletDbContext = walletDbContext;
        }

        public async Task<Unit> Handle(ClaimCommand request, CancellationToken cancellationToken)
        {
            var pair = await _walletDbContext.StakingPairs.FirstOrDefaultAsync(
                t => t.Id == request.Id, cancellationToken);
            
            if(pair == null)
            {
                throw new StakingPairNotFoundException(request.Id);
            }

            if (!pair.IsEnabled)
            {
                throw new StakingIsDisabledException(pair.Id);
            }
            

            var stakes = await _walletDbContext.Stakings.Where(s => s.WalletType == StakingWallet.MOSAICO_WALLET &&
                s.Status == StakingStatus.Deployed && s.UserId == request.UserId && s.StakingPairId == pair.Id).ToListAsync(cancellationToken);
            if (!stakes.Any())
            {
                throw new StakingNotFoundException(pair.Id);
            }

            if (!(await _walletStakingService.CanClaimAsync(stakes.FirstOrDefault())))
            {
                throw new UnableToClaimException(stakes.FirstOrDefault()?.UserId, pair.Id);
            }

            var operation = await _walletDbContext.Operations
                .FirstOrDefaultAsync(t => t.Network == pair.Network && t.Type == BlockchainOperationType.STAKE_CLAIMING &&
                                          t.State == OperationState.IN_PROGRESS
                                          && t.UserId == request.UserId, cancellationToken);
            if (operation != null)
            {
                throw new AnotherStakingInProgressException(request.UserId);
            }
            
            operation = await _operationService.CreateStakeClaimOperationAsync(pair.Network, request.UserId, pair.Id);

            try
            {
                var claimableBalance = await _walletStakingService.GetEstimatedRewardAsync(stakes.FirstOrDefault());
                if (claimableBalance <= 0)
                {
                    throw new NothingToClaimException(stakes.FirstOrDefault()?.UserId, pair.Id);
                }
                var hash = await _walletStakingService.ClaimAsync(stakes.FirstOrDefault());
                await _operationService.SetTransactionInProgress(operation.Id, hash: hash);
                var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                    new ClaimReward(pair.Id, operation.Id, request.UserId, claimableBalance));
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