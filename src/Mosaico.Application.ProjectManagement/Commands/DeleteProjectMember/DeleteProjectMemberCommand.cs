using System;
using MediatR;
using Mosaico.Application.ProjectManagement.Queries.GetProjectMembers;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;
using Mosaico.SDK.Identity.Pipelines;

namespace Mosaico.Application.ProjectManagement.Commands.DeleteProjectMember
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [CacheReset(nameof(GetProjectMembersQuery), "{{ProjectId}}")]
    public class DeleteProjectMemberCommand : IRequest
    {
        public Guid ProjectMemberId { get; set; }
        public Guid ProjectId { get; set; }
    }
}