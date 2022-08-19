using System;
using Mosaico.Domain.ProjectManagement.Entities.Affiliation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.ProjectManagement.DTOs.Affiliation
{
    public class PartnerDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public PartnerPaymentStatus PaymentStatus { get; set; }
        
        public string FailureReason { get; set; }
        public decimal RewardPercentage { get; set; }
        public decimal EstimatedReward { get; set; }
        public long TransactionsCount { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public PartnerStatus Status { get; set; }
    }
}