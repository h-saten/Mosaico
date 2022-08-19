using System;
using MediatR;
using Mosaico.Application.ProjectManagement.Queries.Airdrop.GetProjectAirdrops;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.DistributeAirdrop
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [CacheReset(nameof(GetProjectAirdropsQuery), "{{ProjectId}}")]
    public class DistributeAirdropCommand : IRequest
    {
        public Guid ProjectId { get; set; }
        public Guid AirdropId { get; set; }
    }
}