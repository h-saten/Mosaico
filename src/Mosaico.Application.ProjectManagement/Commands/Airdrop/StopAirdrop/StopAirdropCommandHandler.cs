using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions.Airdrop;
using Mosaico.Base.Tools;
using Mosaico.Domain.ProjectManagement.Abstractions;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.StopAirdrop
{
    public class StopAirdropCommandHandler : IRequestHandler<StopAirdropCommand>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IDateTimeProvider _provider;

        public StopAirdropCommandHandler(IProjectDbContext projectDbContext, IDateTimeProvider provider)
        {
            _projectDbContext = projectDbContext;
            _provider = provider;
        }

        public async Task<Unit> Handle(StopAirdropCommand request, CancellationToken cancellationToken)
        {
            var airdrop = await _projectDbContext.AirdropCampaigns.FirstOrDefaultAsync(a =>
                a.Id == request.AirdropId && a.ProjectId == request.ProjectId, cancellationToken: cancellationToken);
            
            if (airdrop == null)
            {
                throw new AirdropNotFoundException(request.AirdropId.ToString());
            }
            
            airdrop.EndDate = _provider.Now();
            _projectDbContext.AirdropCampaigns.Update(airdrop);
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}