using System;
using MediatR;
using Mosaico.Application.ProjectManagement.Queries.GetProject;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Commands.Administration.UpdateProjectPublicity
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [CacheReset(nameof(GetProjectQuery), "{{ProjectId}}")]
    public class UpdateProjectPublicityCommand : IRequest
    {
        public Guid ProjectId { get; set; }
        public bool IsPublic { get; set; }
    }
}