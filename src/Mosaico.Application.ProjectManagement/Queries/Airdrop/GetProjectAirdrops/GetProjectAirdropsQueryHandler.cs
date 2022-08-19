using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs.Airdrop;
using Mosaico.Base.Tools;
using Mosaico.Domain.ProjectManagement.Abstractions;

namespace Mosaico.Application.ProjectManagement.Queries.Airdrop.GetProjectAirdrops
{
    public class GetProjectAirdropsQueryHandler : IRequestHandler<GetProjectAirdropsQuery, List<AirdropDTO>>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IDateTimeProvider _timeProvider;

        public GetProjectAirdropsQueryHandler(IProjectDbContext projectDbContext, IDateTimeProvider timeProvider)
        {
            _projectDbContext = projectDbContext;
            _timeProvider = timeProvider;
        }

        public async Task<List<AirdropDTO>> Handle(GetProjectAirdropsQuery request, CancellationToken cancellationToken)
        {
            var airdrops = await _projectDbContext.AirdropCampaigns
                .Include(c => c.Participants)
                .AsNoTracking()
                .Where(ac => ac.ProjectId == request.ProjectId).ToListAsync(cancellationToken);
            return airdrops.Select(a =>
            {
                var claimed = a.Participants.Where(p => p.Claimed).Sum(t => t.ClaimedTokenAmount);
                var percentage = a.TotalCap > 0 ? claimed * 100 / a.TotalCap : 0;
                var pendingParticipants = a.Participants.Count(p => p.Claimed && !p.WithdrawnAt.HasValue);
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
                    TokensPerParticipant = a.TokensPerParticipant,
                    ClaimedTokens = claimed,
                    ClaimedPercentage = percentage,
                    PendingParticipants = pendingParticipants,
                    CountAsPurchase = a.CountAsPurchase,
                    TokenId = a.TokenId
                };
            }).ToList();
        }
    }
}