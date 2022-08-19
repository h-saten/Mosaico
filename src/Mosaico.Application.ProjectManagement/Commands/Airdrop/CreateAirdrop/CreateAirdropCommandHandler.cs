using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions.Airdrop;
using Mosaico.Base.Extensions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.SDK.Wallet.Abstractions;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.CreateAirdrop
{
    public class CreateAirdropCommandHandler : IRequestHandler<CreateAirdropCommand, Guid>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IWalletClient _walletClient;

        public CreateAirdropCommandHandler(IProjectDbContext projectDbContext, IWalletClient walletClient)
        {
            _projectDbContext = projectDbContext;
            _walletClient = walletClient;
        }

        public async Task<Guid> Handle(CreateAirdropCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectDbContext.Projects.Include(p => p.AirdropCampaigns).FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            var slug = request.Name.ToSlug().ToLowerInvariant() + Guid.NewGuid().ToString()[..7];
            if (project.AirdropCampaigns.Any(a => a.Slug == slug))
            {
                throw new AirdropCampaignAlreadyExists(request.Name);
            }

            if (!project.TokenId.HasValue)
            {
                throw new NoTokenForAirdropException(project.Title);
            }

            var token = await _walletClient.GetTokenAsync(project.TokenId.Value);
            if (token == null) throw new TokenNotFoundException();
            
            var campaign = new AirdropCampaign
            {
                Name = request.Name,
                Slug = slug,
                Project = project,
                ProjectId = project.Id,
                EndDate = request.EndDate,
                StartDate = request.StartDate,
                IsOpened = request.IsOpened,
                TokenId = project.TokenId.Value,
                TokensPerParticipant = request.TokensPerParticipant,
                TotalCap = request.TotalCap,
                CountAsPurchase = request.CountAsPurchase,
                StageId = request.StageId
            };
            project.AirdropCampaigns.Add(campaign);
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            return campaign.Id;
        }
    }
}