using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.UpdatePartner
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UpdatePartnerCommand : IRequest
    {
        public Guid ProjectId { get; set; }
        public Guid PartnerId { get; set; }
        public decimal RewardPercentage { get; set; }
    }
}