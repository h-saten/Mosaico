using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions.Airdrop;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Validation.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.AddAirdropParticipants
{
    public class AddAirdropParticipantsCommandHandler : IRequestHandler<AddAirdropParticipantsCommand>
    {
        private readonly IProjectDbContext _projectDbContext;

        public AddAirdropParticipantsCommandHandler(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<Unit> Handle(AddAirdropParticipantsCommand request, CancellationToken cancellationToken)
        {
            var airdrop = await _projectDbContext.AirdropCampaigns.Include(p => p.Participants).FirstOrDefaultAsync(a =>
                a.Id == request.AirdropId && a.ProjectId == request.ProjectId, cancellationToken);
            if(airdrop == null)
            {
                throw new AirdropNotFoundException(request.AirdropId.ToString());
            }

            var participants = request.Emails
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();
            if (!participants.Any())
            {
                throw new ValidationException("INVALID_PARTICIPANTS", StatusCodes.Status400BadRequest);
            }
            var shouldSave = false;
            foreach (var participant in participants)
            {
                if (!airdrop.Participants.Any(p => p.Email.ToLowerInvariant() == participant.ToLowerInvariant()))
                {
                    airdrop.Participants.Add(new AirdropParticipant
                    {
                        Email = participant,
                        AirdropCampaign = airdrop,
                        AirdropCampaignId = airdrop.Id
                    });
                    shouldSave = true;
                }
            }

            if (shouldSave)
            {
                await _projectDbContext.SaveChangesAsync(cancellationToken);
            }
            
            return Unit.Value;
        }
    }
}