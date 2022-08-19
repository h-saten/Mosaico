using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.Affiliation
{
    public class ProjectAffiliation : EntityBase
    {
        public ProjectAffiliation()
        {
            IncludeAll = true;
            EverybodyCanParticipate = true;
        }
        
        public bool IsEnabled { get; set; }
        public decimal RewardPercentage { get; set; }
        public decimal RewardPool { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public virtual List<Partner> Partners { get; set; } = new List<Partner>();
        public DateTimeOffset? StartsAt { get; set; }
        public bool IncludeAll { get; set; }
        public bool EverybodyCanParticipate { get; set; }
        public bool PartnerShouldBeInvestor { get; set; }
    }
}