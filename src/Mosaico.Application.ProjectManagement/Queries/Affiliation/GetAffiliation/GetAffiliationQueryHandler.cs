using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Extensions;

namespace Mosaico.Application.ProjectManagement.Queries.Affiliation.GetAffiliation
{
    public class GetAffiliationQueryHandler : IRequestHandler<GetAffiliationQuery, GetAffiliationQueryResponse>
    {
        private readonly IProjectDbContext _projectDbContext;

        public GetAffiliationQueryHandler(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<GetAffiliationQueryResponse> Handle(GetAffiliationQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectDbContext.GetProjectOrThrowAsync(request.ProjectId, cancellationToken);
            if (project.ProjectAffiliation == null) return new GetAffiliationQueryResponse();
            return new GetAffiliationQueryResponse
            {
                Id = project.ProjectAffiliation.Id,
                IsEnabled = project.ProjectAffiliation.IsEnabled,
                RewardPercentage = project.ProjectAffiliation.RewardPercentage,
                RewardPool = project.ProjectAffiliation.RewardPool,
                IncludeAll = project.ProjectAffiliation.IncludeAll,
                EverybodyCanParticipate = project.ProjectAffiliation.EverybodyCanParticipate,
                PartnerShouldBeInvestor = project.ProjectAffiliation.PartnerShouldBeInvestor
            };
        }
    }
}