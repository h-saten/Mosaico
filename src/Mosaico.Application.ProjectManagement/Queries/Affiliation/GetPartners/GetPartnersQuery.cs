using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Queries.Affiliation.GetPartners
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class GetPartnersQuery : IRequest<GetPartnersQueryResponse>
    {
        public Guid ProjectId { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
    }
}