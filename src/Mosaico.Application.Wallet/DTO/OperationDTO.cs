using System;
using Mosaico.Domain.Wallet.Entities.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.Wallet.DTO
{
    public class OperationDTO
    {
        public Guid Id { get; set; }
        public Guid? TransactionId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public BlockchainOperationType Type { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public OperationState State { get; set; }
        
        public DateTimeOffset? FinishedAt { get; set; }
        public int RetryAttempt { get; set; }
        public string TransactionHash { get; set; }
        public string UserId { get; set; }
        public decimal? GasUsed { get; set; }
        public decimal? PayedNativeCurrency { get; set; }
        public decimal? PayedInUSD { get; set; }
        public string Network { get; set; }
        public string AccountAddress { get; set; }
        public string ContractAddress { get; set; }
        public Guid? ProjectId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}