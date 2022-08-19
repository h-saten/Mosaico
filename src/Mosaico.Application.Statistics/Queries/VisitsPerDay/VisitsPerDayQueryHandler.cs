using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Analytics.Base;
using Mosaico.Application.Statistics.Exceptions;
using Mosaico.Base.Tools;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Statistics.Queries.VisitsPerDay
{
    public class VisitsPerDayQueryHandler : IRequestHandler<VisitsPerDayQuery, VisitsPerDayResponse>
    {
        private readonly ITrafficProvider _statisticsProvider;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IDateTimeProvider _dateTime;
        
        public VisitsPerDayQueryHandler(
            ITrafficProvider statisticsProvider, 
            IProjectManagementClient projectManagementClient, 
            IDateTimeProvider dateTime)
        {
            _statisticsProvider = statisticsProvider;
            _projectManagementClient = projectManagementClient;
            _dateTime = dateTime;
        }
        
        public async Task<VisitsPerDayResponse> Handle(VisitsPerDayQuery request, CancellationToken cancellationToken)
        {
            var projectDetails = await _projectManagementClient
                .GetProjectAsync(request.ProjectId, cancellationToken);
            
            if (projectDetails is null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }
            
            var currentDate = _dateTime.Now();
            var stats = await _statisticsProvider.PagesVisitsAsync(projectDetails.Slug, currentDate.AddMonths(-1), currentDate);
            return new VisitsPerDayResponse
            {
                FundPageVisits = stats.FundPageVisits,
                TokenPageVisits = stats.TokenPageVisits
            };
        }
    }
}