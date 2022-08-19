using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;
using Mosaico.Domain.ProjectManagement.Extensions;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.UpdateProjectAffiliation
{
    public class UpdateProjectAffiliationCommandHandler : IRequestHandler<UpdateProjectAffiliationCommand>
    {
        private readonly IProjectDbContext _projectDbContext;

        public UpdateProjectAffiliationCommandHandler(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<Unit> Handle(UpdateProjectAffiliationCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectDbContext.GetProjectOrThrowAsync(request.ProjectId, cancellationToken);
            project.ProjectAffiliation ??= new ProjectAffiliation();
            project.ProjectAffiliation.IsEnabled = request.IsEnabled;
            project.ProjectAffiliation.RewardPercentage = request.RewardPercentage;
            project.ProjectAffiliation.RewardPool = request.RewardPool;
            project.ProjectAffiliation.PartnerShouldBeInvestor = request.PartnerShouldBeInvestor;
            project.ProjectAffiliation.IncludeAll = request.IncludeAll;
            project.ProjectAffiliation.EverybodyCanParticipate = request.EverybodyCanParticipate;
            _projectDbContext.Projects.Update(project);
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}