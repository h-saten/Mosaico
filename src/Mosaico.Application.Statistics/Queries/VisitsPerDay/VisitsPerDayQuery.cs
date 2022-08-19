using System;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Statistics.Queries.VisitsPerDay
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [Cache("{{ProjectId}}")]
    public class VisitsPerDayQuery : IRequest<VisitsPerDayResponse>
    {
        public Guid ProjectId { get; set; }
    }
}