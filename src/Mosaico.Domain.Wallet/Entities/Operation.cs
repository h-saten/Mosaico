using System;
using Mosaico.Domain.Base;
using Mosaico.Domain.Wallet.Entities.Enums;
using Newtonsoft.Json;

namespace Mosaico.Domain.Wallet.Entities
{
    public class Operation : EntityBase
    {
        public Operation()
        {
            Type = BlockchainOperationType.UNKNOWN;
            State = OperationState.PENDING;
        }

        public Guid? TransactionId { get; set; }
        public BlockchainOperationType Type { get; set; }
        public OperationState State { get; set; }
        public DateTimeOffset? FinishedAt { get; set; }
        public string TransactionHash { get; set; }
        public string UserId { get; set; }
        public decimal? GasUsed { get; set; }
        public decimal? PayedNativeCurrency { get; set; }
        public string Network { get; set; }
        public string AccountAddress { get; set; }
        public string ContractAddress { get; set; }
        public Guid? ProjectId { get; set; }
        public string ExtraData { get; set; }
        public string FailureReason { get; set; }
        public Guid? CompanyId { get; set; }

        public T GetData<T>()
        {
            return JsonConvert.DeserializeObject<T>(ExtraData);
        }
    }
}