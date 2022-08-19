using System;
using System.Collections.Generic;

namespace Mosaico.SDK.ProjectManagement.Models
{
    public class MosaicoProjectDetails
    {
        public string Title { get; set; }
        public Guid Id { get; set; }
        public string Slug { get; set; }
        public string CrowdsaleContractAddress { get; set; }
        public string CrowdsaleOwnerAddress { get; set; }
        public string CrowdsaleContractVersion { get; set; }
        public Guid? TokenId { get; set; }
        public string Network { get; set; }
        public Guid? VestingId { get; set; }
        public bool IsVestingEnabled { get; set; }
        public Guid? StakingId { get; set; }
        public bool IsStakingEnabled { get; set; }
        public Guid? CompanyId { get; set; }
        public bool SaleInProgress { get; set; }
        public List<string> PaymentMethods { get; set; } = new();
        public List<ProjectStage> Stages { get; set; } = new List<ProjectStage>();
        public Guid? ActiveStageId { get; set; }
        public decimal SoftCap { get; set; }
        public decimal HardCap { get; set; }
    }
}