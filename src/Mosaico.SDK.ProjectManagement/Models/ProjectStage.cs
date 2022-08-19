using System;
using System.Collections.Generic;

namespace Mosaico.SDK.ProjectManagement.Models
{
    public class ProjectStage
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsPreSale { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public decimal TokensSupply { get; set; }
        public decimal TokenPrice { get; set; }
        public decimal TokenPriceNativeCurrency { get; set; }
        public decimal MinimumPurchase { get; set; }
        public decimal MaximumPurchase { get; set; }
        public Guid ProjectId { get; set; }
        public string Status { get; set; }
        public string AuthorizationCode { get; set; }
        public List<ProjectStageLimit> PurchaseLimits { get; set; } = new List<ProjectStageLimit>();
    }
}