using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs.Airdrop;
using Mosaico.Application.ProjectManagement.Exceptions.Airdrop;
using Mosaico.Authorization.Base;
using Mosaico.Base.Tools;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Wallet.Abstractions;

namespace Mosaico.Application.ProjectManagement.Queries.Airdrop.GetAirdrop
{
    public class GetAirdropQueryHandler : IRequestHandler<GetAirdropQuery, AirdropDTO>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IWalletClient _walletClient;
        private readonly ICurrentUserContext _currentUserContext;

        public GetAirdropQueryHandler(IProjectDbContext projectDbContext, IDateTimeProvider timeProvider, IWalletClient walletClient, ICurrentUserContext currentUserContext)
        {
            _projectDbContext = projectDbContext;
            _timeProvider = timeProvider;
            _walletClient = walletClient;
            _currentUserContext = currentUserContext;
        }

        public async Task<AirdropDTO> Handle(GetAirdropQuery request, CancellationToken cancellationToken)
        {
            var airdropQuery = _projectDbContext.AirdropCampaigns
                .Include(a => a.Participants)
                .Include(p => p.Project)
                .AsNoTracking();
            
            AirdropCampaign a = default;
            if (Guid.TryParse(request.Id, out var airdropGuid))
            {
                a = await airdropQuery.FirstOrDefaultAsync(a => a.Id == airdropGuid, cancellationToken);
            }
            else{
                var slug = request.Id.Trim().ToLowerInvariant();
                a = await airdropQuery.FirstOrDefaultAsync(a => a.Slug == slug, cancellationToken);
            }
            
            if (a == null)
            {
                throw new AirdropNotFoundException(request.Id);
            }
            var claimed = a.Participants.Where(p => p.Claimed).Sum(t => t.ClaimedTokenAmount);
            var percentage = a.TotalCap > 0 ? claimed * 100 / a.TotalCap : 0;
            var pendingParticipants = a.Participants.Count(p => p.Claimed && !p.WithdrawnAt.HasValue);
            var tokenSymbol = string.Empty;
            if (a.Project?.TokenId != null)
            {
                var token = await _walletClient.GetTokenAsync(a.Project.TokenId.Value);
                tokenSymbol = token?.Symbol;
            }

            var tokensToClaim = a.TokensPerParticipant;
            if (_currentUserContext.IsAuthenticated)
            {
                var participantPrefilled = a.Participants.FirstOrDefault(p =>
                    p.Email.Trim().ToLower() == _currentUserContext.Email.Trim().ToLower());
                if (participantPrefilled != null && participantPrefilled.ClaimedTokenAmount > 0)
                    tokensToClaim = participantPrefilled.ClaimedTokenAmount;
            }

            return new AirdropDTO
            {
                Id = a.Id,
                Name = a.Name,
                Slug = a.Slug,
                EndDate = a.EndDate,
                IsOpened = a.IsOpened,
                StartDate = a.StartDate,
                IsFinished = _timeProvider.Now() > a.EndDate,
                TotalCap = a.TotalCap,
                TokensPerParticipant = tokensToClaim,
                ClaimedTokens = claimed,
                ClaimedPercentage = percentage,
                TokenSymbol = tokenSymbol,
                TokenId = a.Project?.TokenId,
                PendingParticipants = pendingParticipants
            };
        }
    }
}