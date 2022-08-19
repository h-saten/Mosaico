using System;
using MediatR;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.Queries.GetProject;
using Mosaico.Application.ProjectManagement.Queries.GetTokenomics;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Commands.UpdateProject
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [CacheReset(nameof(GetProjectQuery), "{{ProjectId}}")]
    [CacheReset(nameof(GetTokenomicsQuery), "{{ProjectId}}")]
    public class UpdateProjectCommand : IRequest
    {
        public Guid ProjectId { get; set; }
        public UpdateProjectDTO Project { get; set; }
    }
}