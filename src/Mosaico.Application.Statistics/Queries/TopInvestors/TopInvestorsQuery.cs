using System;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Statistics.Queries.TopInvestors
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [Cache("{{ProjectId}}")]
    public class TopInvestorsQuery : IRequest<TopInvestorsResponse>
    {
        public Guid ProjectId { get; set; }
    }
}