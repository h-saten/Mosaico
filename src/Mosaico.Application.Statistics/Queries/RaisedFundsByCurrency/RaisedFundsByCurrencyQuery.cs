using System;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Statistics.Queries.RaisedFundsByCurrency
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [Cache("{{ProjectId}}")]
    public class RaisedFundsByCurrencyQuery : IRequest<RaisedFundsByCurrencyResponse>
    {
        public Guid ProjectId { get; set; }
    }
}