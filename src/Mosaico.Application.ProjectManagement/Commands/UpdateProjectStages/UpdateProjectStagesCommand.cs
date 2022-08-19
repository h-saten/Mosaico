using System;
using System.Collections.Generic;
using MediatR;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.Queries.GetProjectStages;
using Mosaico.Application.ProjectManagement.Queries.GetTokenomics;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Commands.UpdateProjectStages
{
    [Restricted(nameof(Id), Authorization.Base.Constants.Permissions.Project.CanEditStages)]
    [CacheReset(nameof(GetProjectStagesQuery), "{{Id}}")]
    [CacheReset(nameof(GetTokenomicsQuery), "{{Id}}")]
    public class UpdateProjectStagesCommand : IRequest<List<StageDTO>>
    {
        public Guid Id { get; set; }
        public List<StageCreationDTO> Stages = new List<StageCreationDTO>();
    }
}