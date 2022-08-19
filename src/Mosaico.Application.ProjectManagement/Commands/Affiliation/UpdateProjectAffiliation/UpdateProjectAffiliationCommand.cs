using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.Affiliation.UpdateProjectAffiliation
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UpdateProjectAffiliationCommand : IRequest
    {
        public Guid ProjectId { get; set; }
        public bool IsEnabled { get; set; }
        public decimal RewardPercentage { get; set; }
        public decimal RewardPool { get; set; }
        public bool IncludeAll { get; set; }
        public bool EverybodyCanParticipate { get; set; }
        public bool PartnerShouldBeInvestor { get; set; }
    }
}