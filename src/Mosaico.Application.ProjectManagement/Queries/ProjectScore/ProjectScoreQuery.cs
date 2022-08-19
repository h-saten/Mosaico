using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Queries.ProjectScore
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class ProjectScoreQuery : IRequest<ProjectScoreQueryResponse>
    {
        public Guid ProjectId { get; set; }
    }
}