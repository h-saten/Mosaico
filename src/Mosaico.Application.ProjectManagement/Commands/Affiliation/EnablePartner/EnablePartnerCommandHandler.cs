using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Commands.Affiliation.UpdatePartner;
using Mosaico.Application.ProjectManagement.Exceptions.Affiliation;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.EnablePartner
{
    public class EnablePartnerCommandHandler : IRequestHandler<EnablePartnerCommand>
    {
        private readonly IProjectDbContext _projectDbContext;

        public EnablePartnerCommandHandler(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<Unit> Handle(EnablePartnerCommand request, CancellationToken cancellationToken)
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

            partner.Status = PartnerStatus.ENABLED;
            
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}