using System;

namespace Mosaico.SDK.ProjectManagement.Models
{
    public class MosaicoStakingFund
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Distribution { get; set; }
        public decimal DistributionPerPerson { get; set; }
        public long Days { get; set; }
        public DateTimeOffset? StartAt { get; set; }
        public DateTimeOffset? EndAt { get; set; }
        public bool CanWithdrawEarly { get; set; }
        public long? SubtractedPercent { get; set; }
        public Guid? StakingWalletId { get; set; }
    }
}