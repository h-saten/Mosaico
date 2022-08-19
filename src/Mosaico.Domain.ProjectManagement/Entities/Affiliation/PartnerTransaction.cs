using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.Affiliation
{
    public class PartnerTransaction : EntityBase
    {
        public Guid PartnerId { get; set; }
        public virtual Partner Partner { get; set; }
        public string TransactionCorrelationId { get; set; }
        public decimal PurchasedTokens { get; set; }
        public decimal EstimatedReward { get; set; }
        public string PurchasedById { get; set; }
    }
}