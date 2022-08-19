using System;
using System.Collections.Generic;
using MediatR;
using Mosaico.Application.ProjectManagement.DTOs.Airdrop;
using Mosaico.Authorization.Base;
using Mosaico.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.ProjectManagement.Queries.Airdrop.GetAirdropParticipants
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class GetAirdropParticipantsQuery : IRequest<PaginatedResult<AirdropParticipantDTO>>
    {
        public Guid ProjectId { get; set; }
        public Guid AirdropId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string SearchText { get; set; }
    }
}