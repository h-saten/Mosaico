using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.Affiliation
{
    public class Partner : EntityBase
    {
        public string AccessCode { get; set; }
        public Guid ProjectAffiliationId { get; set; }
        public virtual ProjectAffiliation ProjectAffiliation { get; set; }
        public PartnerStatus Status { get; set; }
        public Guid UserAffiliationId { get; set; }
        public virtual UserAffiliation UserAffiliation { get; set; }
        public PartnerPaymentStatus PaymentStatus { get; set; }
        public string FailureReason { get; set; }
        public decimal RewardPercentage { get; set; }
        public virtual List<PartnerTransaction> PartnerTransactions { get; set; } = new List<PartnerTransaction>();
    }
}