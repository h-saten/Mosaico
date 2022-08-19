using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectSubscribers
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class GetProjectSubscribersQuery : IRequest<GetProjectSubscribersQueryResponse>
    {
        public Guid ProjectId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}