using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class AirdropParticipant : EntityBase
    {
        public string Email { get; set; }
        public string WalletAddress { get; set; }
        public bool Claimed { get; set; }
        public DateTimeOffset? ClaimedAt { get; set; }
        public virtual AirdropCampaign AirdropCampaign { get; set; }
        public Guid AirdropCampaignId { get; set; }
        public decimal ClaimedTokenAmount { get; set; }
        public DateTimeOffset? WithdrawnAt { get; set; }
        public string TransactionHash { get; set; }
        public string UserId { get; set; }
    }
}