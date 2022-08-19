using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.ApplyUserReference
{
    public class ApplyUserReferenceCommandHandler : IRequestHandler<ApplyUserReferenceCommand>
    {
        private readonly IProjectDbContext _projectDbContext;

        public ApplyUserReferenceCommandHandler(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<Unit> Handle(ApplyUserReferenceCommand request, CancellationToken cancellationToken)
        {
            if (request.ProjectId.HasValue)
            {
                var projectExists = await _projectDbContext.Projects.AnyAsync(t => t.Id == request.ProjectId, cancellationToken: cancellationToken);
                if (!projectExists) throw new ProjectNotFoundException(request.ProjectId.Value);
            }
            
            var normalizedCode = request.RefCode?.ToLowerInvariant().Trim();
            var userAffiliation = await _projectDbContext.UserAffiliations.FirstOrDefaultAsync(a => a.AccessCode == normalizedCode, cancellationToken);
            if (userAffiliation != null)
            {
                var reference = await _projectDbContext.AffiliationReferences.FirstOrDefaultAsync(t =>
                    t.UserAffiliationId == userAffiliation.Id && t.ReferencedUserId == request.UserId && (request.ProjectId == null || request.ProjectId  == t.ProjectId), cancellationToken: cancellationToken);
                if (reference == null)
                {
                    _projectDbContext.AffiliationReferences.Add(new UserAffiliationReference
                    {
                        ProjectId = request.ProjectId,
                        UserAffiliation = userAffiliation,
                        UserAffiliationId = userAffiliation.Id,
                        ReferencedUserId = request.UserId
                    });
                    await _projectDbContext.SaveChangesAsync(cancellationToken);
                }
            }
            return Unit.Value;
        }
    }
}