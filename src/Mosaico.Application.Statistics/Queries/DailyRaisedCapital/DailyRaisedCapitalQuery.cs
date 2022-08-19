using System;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Statistics.Queries.DailyRaisedCapital
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [Cache("{{ProjectId}}")]
    public class DailyRaisedCapitalQuery : IRequest<DailyRaisedCapitalResponse>
    {
        public Guid ProjectId { get; set; }
        public int FromMonthAgo { get; set; }
    }
}