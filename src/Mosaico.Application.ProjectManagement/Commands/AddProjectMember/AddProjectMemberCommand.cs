using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Application.ProjectManagement.Queries.GetProjectMembers;
using Mosaico.Application.ProjectManagement.Queries.TokenPage.GetTokenPage;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;
using Mosaico.SDK.Identity.Pipelines;

namespace Mosaico.Application.ProjectManagement.Commands.AddProjectMember
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [CacheReset(nameof(GetProjectMembersQuery), "{{ProjectId}}")]
    public class AddProjectMemberCommand : IRequest
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}