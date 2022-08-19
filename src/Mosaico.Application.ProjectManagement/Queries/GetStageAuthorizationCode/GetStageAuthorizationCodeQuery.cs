using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Queries.GetStageAuthorizationCode
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class GetStageAuthorizationCodeQuery : IRequest<string>
    {
        public Guid ProjectId { get; set; }
        public Guid StageId { get; set; }
    }
}