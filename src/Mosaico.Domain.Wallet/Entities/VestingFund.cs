using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;
using Mosaico.Domain.Wallet.Entities.Enums;

namespace Mosaico.Domain.Wallet.Entities
{
    public class VestingFund : EntityBase
    {
        public string Name { get; set; }
        public decimal TokenAmount { get; set; }
        public int Days { get; set; }
        public DateTimeOffset? StartAt { get; set; }
        public virtual Vesting Vesting { get; set; }
        public Guid VestingId { get; set; }
        public string SmartContractId { get; set; }
        public VestingFundStatus Status { get; set; }
        public string FailureReason { get; set; }
        public string ApprovalTransactionHash { get; set; }
        public string TransactionHash { get;set; }
    }
}