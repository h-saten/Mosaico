using System;
using System.Collections.Generic;
using MediatR;
using Mosaico.Application.ProjectManagement.DTOs.Airdrop;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.Airdrop.GetProjectAirdrops
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class GetProjectAirdropsQuery : IRequest<List<AirdropDTO>>
    {
        public Guid ProjectId { get; set; }
    }
}