using System;

namespace Mosaico.SDK.ProjectManagement.Models
{
    public class MosaicoStaking
    {
        public Guid Id { get; set; }
        public Guid TokenId { get; set; }
        public Guid ProjectId { get; set; }
        public string ContractAddress { get; set; }
        public int RewardDistributionInDays { get; set; }
        public decimal RewardPercentage { get; set; }
        public decimal RewardsDeposit { get; set; }
    }
}