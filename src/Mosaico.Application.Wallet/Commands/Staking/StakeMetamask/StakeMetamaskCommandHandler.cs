using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;

namespace Mosaico.Application.Wallet.Commands.Staking.StakeMetamask
{
    public class StakeMetamaskCommandHandler : IRequestHandler<StakeMetamaskCommand>
    {
        private readonly IWalletDbContext _dbContext;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IEventPublisher _publisher;
        private readonly IEventFactory _eventFactory;

        public StakeMetamaskCommandHandler(IWalletDbContext dbContext, ICurrentUserContext currentUserContext, IEventPublisher publisher, IEventFactory eventFactory)
        {
            _dbContext = dbContext;
            _currentUserContext = currentUserContext;
            _publisher = publisher;
            _eventFactory = eventFactory;
        }

        public async Task<Unit> Handle(StakeMetamaskCommand request, CancellationToken cancellationToken)
        {
            var stakingPair = await _dbContext.StakingPairs.FirstOrDefaultAsync(t => t.Id == request.StakingPairId, cancellationToken);
            
            if (stakingPair == null)
            {
                throw new StakingPairNotFoundException(request.StakingPairId);
            }
            
            var staking = new Domain.Wallet.Entities.Staking.Staking
            {
                Balance = request.Balance,
                Days = request.Days,
                Status = StakingStatus.Deploying,
                StakingPairId = stakingPair.Id,
                StakingPair = stakingPair,
                UserId = _currentUserContext.UserId,
                Wallet = request.Wallet.Trim().ToLowerInvariant(),
                TransactionHash = request.TransactionHash,
                WalletType = StakingWallet.METAMASK
            };
            _dbContext.Stakings.Add(staking);
            await _dbContext.SaveChangesAsync(cancellationToken);
            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets, new MetamaskStakeInitiated(staking.Id, _currentUserContext.UserId));
            await _publisher.PublishAsync(e);
            return Unit.Value;
        }
    }
}