using System;

namespace Mosaico.Application.ProjectManagement.DTOs.Airdrop
{
    public class AirdropDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsOpened { get; set; }
        public decimal TotalCap { get; set; }
        public decimal TokensPerParticipant { get; set; }
        public string Slug { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool IsFinished { get; set; }
        public decimal ClaimedTokens { get; set; }
        public decimal ClaimedPercentage { get; set; }
        public string TokenSymbol { get; set; }
        public Guid? TokenId { get; set; }
        public decimal PendingParticipants { get; set; }
        public bool CountAsPurchase { get; set; }
    }
}