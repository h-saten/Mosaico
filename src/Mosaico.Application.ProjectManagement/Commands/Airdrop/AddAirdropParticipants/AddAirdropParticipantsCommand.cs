using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.AddAirdropParticipants
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class AddAirdropParticipantsCommand : IRequest
    {
        public string Emails { get; set; }
        public Guid ProjectId { get; set; }
        public Guid AirdropId { get; set; }
    }
}