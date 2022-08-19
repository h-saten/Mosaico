using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.Wallet.DTO
{
    public enum VestingStatus
    {
        Pending = 0,
        InProgress,
        Claimed
    }
    
    public class VestingDTO
    {
        public Guid Id { get; set; }
        public List<VestingFundDTO> Funds { get; set; }
        public Guid TokenId { get; set; }
        public DateTimeOffset? StartsAt { get; set; }
        public string Name { get; set; }
        public decimal TokenAmount { get; set; }
        public int NumberOfDays { get; set; }
        public string WalletAddress { get; set; }
        public decimal InitialPaymentPercentage { get; set; }
        public decimal Claimed { get; set; }
        public int TransactionCount { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public VestingStatus Status { get; set; }
        public decimal PercentageCompleted { get; set; }
    }
}