using System;

namespace Mosaico.Application.ProjectManagement.DTOs.Airdrop
{
    public class AirdropParticipantDTO
    {
        public string Email { get; set; }
        public string WalletAddress { get; set; }
        public bool Claimed { get; set; }
        public DateTimeOffset? ClaimedAt { get; set; }
        public decimal ClaimedTokenAmount { get; set; }
        public DateTimeOffset? WithdrawnAt { get; set; }
        public string TransactionHash { get; set; }
    }
}