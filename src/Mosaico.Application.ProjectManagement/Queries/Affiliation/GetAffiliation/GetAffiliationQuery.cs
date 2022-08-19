using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Queries.Affiliation.GetAffiliation
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class GetAffiliationQuery : IRequest<GetAffiliationQueryResponse>
    {
        public Guid ProjectId { get; set; }
    }
}