using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;

namespace Mosaico.Application.Wallet.Commands.Staking.WithdrawMetamask
{
    public class WithdrawMetamaskCommandHandler : IRequestHandler<WithdrawMetamaskCommand>
    {
        private readonly IWalletDbContext _dbContext;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;

        public WithdrawMetamaskCommandHandler(IWalletDbContext dbContext, ICurrentUserContext currentUserContext, IEventPublisher eventPublisher, IEventFactory eventFactory)
        {
            _dbContext = dbContext;
            _currentUserContext = currentUserContext;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
        }

        public async Task<Unit> Handle(WithdrawMetamaskCommand request, CancellationToken cancellationToken)
        {
            var pair = await _dbContext.StakingPairs.FirstOrDefaultAsync(
                t => t.Id == request.Id, cancellationToken);
            
            if(pair == null)
            {
                throw new StakingPairNotFoundException(request.Id);
            }

            var normalizedWallet = request.Wallet.Trim().ToLowerInvariant();
            var stakes = await _dbContext.Stakings.Where(s =>
                s.Status == StakingStatus.Deployed && s.UserId == request.UserId && s.StakingPairId == pair.Id
                && s.WalletType == StakingWallet.METAMASK && s.Wallet == normalizedWallet).ToListAsync(cancellationToken);
            if (!stakes.Any())
            {
                throw new StakingNotFoundException(pair.Id);
            }

            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new MetamaskWithdrawalInitiated(pair.Id, _currentUserContext.UserId, request.TransactionHash, request.Wallet));
            await _eventPublisher.PublishAsync(e);
            
            return Unit.Value;
        }
    }
}