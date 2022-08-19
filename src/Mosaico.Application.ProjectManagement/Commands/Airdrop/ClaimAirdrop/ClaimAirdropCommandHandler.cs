using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions.Airdrop;
using Mosaico.Authorization.Base;
using Mosaico.Base.Tools;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.SDK.Wallet.Abstractions;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.ClaimAirdrop
{
    public class ClaimAirdropCommandHandler : IRequestHandler<ClaimAirdropCommand>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ICurrentUserContext _currentUser;
        private readonly IWalletClient _walletClient;
        private readonly IDateTimeProvider _provider;

        public ClaimAirdropCommandHandler(IProjectDbContext projectDbContext, ICurrentUserContext currentUser, IWalletClient walletClient, IDateTimeProvider provider)
        {
            _projectDbContext = projectDbContext;
            _currentUser = currentUser;
            _walletClient = walletClient;
            _provider = provider;
        }

        public async Task<Unit> Handle(ClaimAirdropCommand request, CancellationToken cancellationToken)
        {
            var airdrop = await _projectDbContext.AirdropCampaigns
                .Include(p => p.Participants)
                .Include(a => a.Project)
                .FirstOrDefaultAsync(a => a.Id == request.AirdropId, cancellationToken);
            if (airdrop == null)
            {
                throw new AirdropNotFoundException(request.AirdropId.ToString());
            }
            var now = _provider.Now();
            
            if (now < airdrop.StartDate || now > airdrop.EndDate)
            {
                throw new ForbiddenToClaimException();
            }

            var participant = airdrop.Participants.FirstOrDefault(p =>
                p.Email.Trim().ToUpperInvariant() == _currentUser.Email.Trim().ToUpperInvariant());
            
            if (participant != null && participant.Claimed)
            {
                throw new AirdropAlreadyClaimException();
            }
            if (participant == null && !airdrop.IsOpened)
            {
                throw new ForbiddenToClaimException();
            }

            var sum = airdrop.Participants.Sum(p => p.ClaimedTokenAmount);
            
            
            if (participant == null)
            {
                if (airdrop.TotalCap - sum < airdrop.TokensPerParticipant)
                {
                    throw new AirdropExhaustedException(airdrop.Name);
                }
                
                participant = new AirdropParticipant
                {
                    Email = _currentUser.Email,
                    AirdropCampaign = airdrop,
                    AirdropCampaignId = airdrop.Id,
                    UserId = _currentUser.UserId,
                    ClaimedTokenAmount = airdrop.TokensPerParticipant
                };
                airdrop.Participants.Add(participant);
            }
            else
            {
                participant.UserId = _currentUser.UserId;
            }

            if (!airdrop.Project.TokenId.HasValue)
            {
                throw new TokenNotFoundException(airdrop.ProjectId);
            }
            var token = await _walletClient.GetTokenAsync(airdrop.Project.TokenId.Value);
            if (token == null)
            {
                throw new TokenNotFoundException(airdrop.Project.TokenId.Value);
            }
            var userWallet = await _walletClient.GetUserWalletAsync(_currentUser.UserId, token.Network);
            if (userWallet == null)
            {
                throw new WalletNotFoundException(_currentUser.UserId);
            }

            participant.WalletAddress = userWallet.AccountAddress;
            participant.ClaimedTokenAmount = participant.ClaimedTokenAmount <= 0 ? airdrop.TokensPerParticipant : participant.ClaimedTokenAmount;
            participant.Claimed = true;
            participant.ClaimedAt = _provider.Now();
            
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}