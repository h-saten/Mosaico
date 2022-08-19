using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Entities.Staking;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.SDK.Identity.Abstractions;

namespace Mosaico.Application.Wallet.Commands.Staking.ClaimMetamask
{
    public class ClaimMetamaskCommandHandler : IRequestHandler<ClaimMetamaskCommand>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IEventPublisher _publisher;
        private readonly IEventFactory _eventFactory;

        public ClaimMetamaskCommandHandler(IWalletDbContext dbContext, IEventPublisher publisher, IEventFactory eventFactory)
        {
            _walletDbContext = dbContext;
            _publisher = publisher;
            _eventFactory = eventFactory;
        }

        public async Task<Unit> Handle(ClaimMetamaskCommand request, CancellationToken cancellationToken)
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

            var walletNormalized = request.Wallet.Trim().ToLowerInvariant();
            var stakes = await _walletDbContext.Stakings.Where(s => s.WalletType == StakingWallet.METAMASK && s.Wallet == walletNormalized &&
                                                                    s.Status == StakingStatus.Deployed && s.UserId == request.UserId && s.StakingPairId == pair.Id)
                .ToListAsync(cancellationToken);
            
            if (!stakes.Any())
            {
                throw new StakingNotFoundException(pair.Id);
            }

            var @e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new MetamaskClaimInitiated(pair.Id, request.UserId, request.TransactionHash, request.Wallet, request.Amount));
            await _publisher.PublishAsync(@e);
            
            return Unit.Value;
        }
    }
}