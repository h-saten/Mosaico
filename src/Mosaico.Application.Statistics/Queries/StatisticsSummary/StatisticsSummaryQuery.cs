using System;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Statistics.Queries.StatisticsSummary
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [Cache("{{ProjectId}}")]
    public class StatisticsSummaryQuery : IRequest<StatisticsSummaryResponse>
    {
        public Guid ProjectId { get; set; }
    }
}