using System;
using Mosaico.Domain.ProjectManagement.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class StageDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public StageType Type { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public decimal TokensSupply { get; set; }
        public decimal TokenPrice { get; set; }
        public decimal TokenPriceNativeCurrency { get; set; }
        public decimal MinimumPurchase { get; set; }
        public decimal MaximumPurchase { get; set; }
        public string Status { get; set; }
        public int Order { get; set; }
        public decimal SoldTokens { get; set; }
        public decimal ProgressPercentage { get; set; }
        public Guid ProjectId { get; set; }
    }
}