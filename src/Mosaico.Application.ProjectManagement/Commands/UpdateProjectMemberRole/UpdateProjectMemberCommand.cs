using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.ProjectManagement.Queries.GetProjectMembers;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;
using Mosaico.SDK.Identity.Pipelines;

namespace Mosaico.Application.ProjectManagement.Commands.UpdateProjectMemberRole
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [CacheReset(nameof(GetProjectMembersQuery), "{{ProjectId}}")]
    public class UpdateProjectMemberCommand : IRequest
    {
        public string Role { get; set; }
        
        [JsonIgnore]
        public Guid MemberId { get; set; }
        
        [JsonIgnore]
        public Guid ProjectId { get; set; }
    }
}