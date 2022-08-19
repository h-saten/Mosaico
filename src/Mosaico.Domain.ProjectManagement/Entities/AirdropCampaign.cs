using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class AirdropCampaign : EntityBase
    {
        public string Name { get; set; }
        public Guid TokenId { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public virtual List<AirdropParticipant> Participants { get; set; } = new List<AirdropParticipant>();
        public string Slug { get; set; }
        public bool IsOpened { get; set; }
        public decimal TotalCap { get; set; }
        public decimal TokensPerParticipant { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public bool CountAsPurchase { get; set; }
        public Guid? StageId { get; set; }
    }
}