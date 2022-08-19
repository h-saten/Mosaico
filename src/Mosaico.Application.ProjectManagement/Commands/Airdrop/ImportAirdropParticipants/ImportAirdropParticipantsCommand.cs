using System;
using MediatR;
using Mosaico.Application.ProjectManagement.Queries.Airdrop.GetAirdropParticipants;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.ImportAirdropParticipants
{
    [CacheReset(nameof(GetAirdropParticipantsQuery), "{{ProjectId}}_{{AirdropId}}")]
    public class ImportAirdropParticipantsCommand : IRequest
    {
        public byte[] File { get; set; }
        public Guid AirdropId { get; set; }
        public Guid ProjectId { get; set; }
    }
}