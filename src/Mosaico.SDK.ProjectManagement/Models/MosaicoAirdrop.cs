using System;
using System.Collections.Generic;

namespace Mosaico.SDK.ProjectManagement.Models
{
    public class MosaicoAirdropParticipant
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string WalletAddress { get; set; }
        public bool Claimed { get; set; }
        public DateTimeOffset? ClaimedAt { get; set; }
        public DateTimeOffset? WithdrawnAt { get; set; }
        public decimal ClaimedTokenAmount { get; set; }
    }
    
    public class MosaicoAirdrop
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsOpened { get; set; }
        public decimal TotalCap { get; set; }
        public decimal TokensPerParticipant { get; set; }
        public string Slug { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public Guid ProjectId { get; set; }
        public List<MosaicoAirdropParticipant> Participants = new List<MosaicoAirdropParticipant>();
        public Guid? TokenId { get; set; }
        public bool CountAsPurchase { get; set; }
        public Guid? StageId { get; set; }
    }
}