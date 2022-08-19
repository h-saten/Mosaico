using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs.Airdrop;
using Mosaico.Application.ProjectManagement.Exceptions.Airdrop;
using Mosaico.Authorization.Base;
using Mosaico.Authorization.Base.Exceptions;
using Mosaico.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Extensions;

namespace Mosaico.Application.ProjectManagement.Queries.Airdrop.GetAirdropParticipants
{
    public class GetAirdropParticipantsQueryHandler : IRequestHandler<GetAirdropParticipantsQuery, PaginatedResult<AirdropParticipantDTO>>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IUserManagementClient _userManagementClient;
        private readonly ICurrentUserContext _currentUserContext;

        public GetAirdropParticipantsQueryHandler(IProjectDbContext projectDbContext, IUserManagementClient userManagementClient, ICurrentUserContext currentUserContext)
        {
            _projectDbContext = projectDbContext;
            _userManagementClient = userManagementClient;
            _currentUserContext = currentUserContext;
        }

        public async Task<PaginatedResult<AirdropParticipantDTO>> Handle(GetAirdropParticipantsQuery request, CancellationToken cancellationToken)
        {
            var airdrop = await _projectDbContext.AirdropCampaigns
                .FirstOrDefaultAsync(a => a.Id == request.AirdropId && a.ProjectId == request.ProjectId,
                    cancellationToken);
            if (airdrop == null)
            {
                throw new AirdropNotFoundException(request.AirdropId.ToString());
            }

            var userPermissions = await _userManagementClient.GetUserPermissionsAsync(_currentUserContext.UserId,
                airdrop.TokenId, cancellationToken);
            if (!userPermissions.Any(up => up.Key == Authorization.Base.Constants.Permissions.Token.CanEdit) && !_currentUserContext.IsGlobalAdmin)
                throw new UnauthorizedException($"User is unauthorized to update airdrop {request.AirdropId}");

            var participantsQuery = _projectDbContext.AirdropParticipants.OrderBy(p => p.Email).Where(p => p.AirdropCampaignId == airdrop.Id);
            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                participantsQuery = participantsQuery.Where(t => t.Email.Contains(request.SearchText));
            }

            var items = await participantsQuery.Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken);
            var count = await participantsQuery.CountAsync(cancellationToken);

            return new PaginatedResult<AirdropParticipantDTO>
            {
                Entities = items.Select(p => new AirdropParticipantDTO
                {
                    Claimed = p.Claimed,
                    Email = p.Email,
                    ClaimedAt = p.ClaimedAt,
                    TransactionHash = p.TransactionHash,
                    WalletAddress = p.WalletAddress,
                    WithdrawnAt = p.WithdrawnAt,
                    ClaimedTokenAmount = p.ClaimedTokenAmount
                }).ToList(),
                Total = count
            };
        }
    }
}