using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions.Affiliation;
using Mosaico.Domain.ProjectManagement.Abstractions;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.UpdatePartner
{
    public class UpdatePartnerCommandHandler : IRequestHandler<UpdatePartnerCommand>
    {
        private readonly IProjectDbContext _projectDbContext;

        public UpdatePartnerCommandHandler(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<Unit> Handle(UpdatePartnerCommand request, CancellationToken cancellationToken)
        {
            var affiliation =
                await _projectDbContext.ProjectAffiliations.FirstOrDefaultAsync(p => p.ProjectId == request.ProjectId, cancellationToken);
            if(affiliation == null) return Unit.Value;
            
            var partner = await _projectDbContext.Partners.FirstOrDefaultAsync(
                p => p.ProjectAffiliationId == affiliation.Id && p.Id == request.PartnerId, cancellationToken);
            
            if(partner == null)
            {
                throw new PartnerNotFoundException(request.PartnerId);
            }

            partner.RewardPercentage = request.RewardPercentage;
            
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}