using System;

namespace Mosaico.SDK.ProjectManagement.Models
{
    public class ProjectStageLimit
    {
        public Guid Id { get; set; }
        public string PaymentMethod { get; set; }
        public decimal MinimumPurchase { get; set; }
        public decimal MaximumPurchase { get; set; }
    }
}