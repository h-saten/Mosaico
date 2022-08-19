using System;
using MediatR;
using Mosaico.Application.ProjectManagement.Queries.Airdrop.GetProjectAirdrops;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.CreateAirdrop
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [CacheReset(nameof(GetProjectAirdropsQuery), "{{ProjectId}}")]
    public class CreateAirdropCommand : IRequest<Guid>
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public decimal TotalCap { get; set; }
        public decimal TokensPerParticipant { get; set; }
        public bool IsOpened { get; set; }
        public bool CountAsPurchase { get; set; }
        public Guid? StageId { get; set; }
    }
}