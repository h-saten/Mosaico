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
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.Staking.Distribute
{
    public class DistributeCommandHandler : IRequestHandler<DistributeCommand>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IOperationService _operationService;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWalletStakingService _walletStakingService;
        private readonly ILogger _logger;

        public DistributeCommandHandler(ILogger logger, IWalletStakingService walletStakingService, IEventPublisher eventPublisher, IEventFactory eventFactory, IOperationService operationService, IWalletDbContext walletDbContext)
        {
            _logger = logger;
            _walletStakingService = walletStakingService;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _operationService = operationService;
            _walletDbContext = walletDbContext;
        }

        public async Task<Unit> Handle(DistributeCommand request, CancellationToken cancellationToken)
        {
            var pair = await _walletDbContext.StakingPairs.FirstOrDefaultAsync(
                t => t.Id == request.StakingPairId, cancellationToken);
            
            if(pair == null)
            {
                throw new StakingPairNotFoundException(request.StakingPairId);
            }
            
            if (!pair.IsEnabled)
            {
                throw new StakingIsDisabledException(pair.Id);
            }

            var operation = await _walletDbContext.Operations
                .FirstOrDefaultAsync(t => t.Network == pair.Network && t.Type == BlockchainOperationType.STAKE_DISTRIBUTE &&
                                          t.State == OperationState.IN_PROGRESS
                                          && t.UserId == request.UserId, cancellationToken);
            if (operation != null)
            {
                throw new AnotherStakingInProgressException(request.UserId);
            }
            
            operation = await _operationService.CreateStakeDistributionOperationAsync(pair.Network, request.UserId, pair.Id);
            try
            {
                var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                    new Events.Wallet.Distribute(pair.Id, operation.Id, request.UserId, request.Amount, request.CompanyId));
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