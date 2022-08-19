using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class StagePurchaseLimit : EntityBase
    {
        public Guid StageId { get; set; }
        public virtual Stage Stage { get; set; }
        public string PaymentMethod { get; set; }
        public decimal MinimumPurchase { get; set; }
        public decimal MaximumPurchase { get; set; }
    }
}