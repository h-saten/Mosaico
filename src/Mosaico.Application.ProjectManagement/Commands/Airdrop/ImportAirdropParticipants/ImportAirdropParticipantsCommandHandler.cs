using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Abstractions;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.ProjectManagement.Exceptions.Airdrop;
using Mosaico.Authorization.Base;
using Mosaico.Authorization.Base.Exceptions;
using Mosaico.Base.Tools;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.SDK.Identity.Abstractions;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.ImportAirdropParticipants
{
    public class ImportAirdropParticipantsCommandHandler : IRequestHandler<ImportAirdropParticipantsCommand>
    {
        private readonly IAirdropImportService _airdropService;
        private readonly IUserManagementClient _managementClient;
        private readonly IProjectDbContext _projectDbContext;
        private readonly ICurrentUserContext _currentUser;
        private readonly IDateTimeProvider _timeProvider;
        public ImportAirdropParticipantsCommandHandler(IUserManagementClient managementClient, IAirdropImportService airdropService, IProjectDbContext projectDbContext, ICurrentUserContext currentUser, IDateTimeProvider timeProvider)
        {
            _managementClient = managementClient;
            _airdropService = airdropService;
            _projectDbContext = projectDbContext;
            _currentUser = currentUser;
            _timeProvider = timeProvider;
        }

        public async Task<Unit> Handle(ImportAirdropParticipantsCommand request, CancellationToken cancellationToken)
        {
            var airdrop = await _projectDbContext.AirdropCampaigns.FirstOrDefaultAsync(a => a.Id == request.AirdropId, cancellationToken);
            if (airdrop == null) throw new AirdropNotFoundException(request.AirdropId.ToString());

            var userPermissions = await _managementClient.GetUserPermissionsAsync(_currentUser.UserId, airdrop.TokenId, cancellationToken);
            if (!userPermissions.Any(up => up.Key == Authorization.Base.Constants.Permissions.Token.CanEdit) && !_currentUser.IsGlobalAdmin)
                throw new UnauthorizedException($"User is unauthorized to update airdrop {request.AirdropId}");

            if (_timeProvider.Now() > airdrop.EndDate)
                throw new AirdropExhaustedException(request.AirdropId.ToString());
            
            var participants = await _airdropService.GetAirdropParticipantsAsync(request.File);
            if(!participants.Any())
            {
                throw new AirdropImportFailedException($"There are no participants in file");
            }

            if (participants.Count > 10000)
            {
                throw new AirdropImportFailedException($"There are too many participants");
            }

            var validParticipants = participants.Where(p => !string.IsNullOrWhiteSpace(p.Email) && p.Amount > 0);
            var currentParticipantTotalAmount = airdrop.Participants.Sum(p => p.ClaimedTokenAmount);
            if (currentParticipantTotalAmount > airdrop.TotalCap)
                throw new AirdropExhaustedException(request.AirdropId.ToString());
            foreach (var dto in validParticipants)
            {
                var newParticipantEmail = dto.Email.Trim().ToLower();
                var participant =
                    airdrop.Participants.FirstOrDefault(p =>
                        p.Email.Trim().ToLower() == newParticipantEmail);
                
                if (dto.Amount > airdrop.TokensPerParticipant) throw new AirdropExhaustedException(airdrop.Id.ToString());
                
                if (participant != null && !participant.Claimed)
                {
                    participant.ClaimedTokenAmount = dto.Amount;
                    _projectDbContext.AirdropParticipants.Update(participant);
                }
                else
                {
                    participant = new AirdropParticipant
                    {
                        Email = newParticipantEmail,
                        Claimed = false,
                        AirdropCampaign = airdrop,
                        AirdropCampaignId = airdrop.Id,
                        ClaimedTokenAmount = dto.Amount
                    };
                    airdrop.Participants.Add(participant);
                    _projectDbContext.AirdropCampaigns.Update(airdrop);
                }

                await _projectDbContext.SaveChangesAsync(cancellationToken);
                currentParticipantTotalAmount = airdrop.Participants.Sum(p => p.ClaimedTokenAmount);
                if (currentParticipantTotalAmount > airdrop.TotalCap)
                    throw new AirdropExhaustedException(request.AirdropId.ToString());
            }
            
            return Unit.Value;
        }
    }
}