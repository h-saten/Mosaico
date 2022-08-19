using System;

namespace Mosaico.Application.Wallet.DTO
{
    public class VestingUserInvitationDTO
    {
        public Guid Id { get; set; }
        public bool IsAccepted { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectTitle { get; set; }
        public string TokenName { get; set; }
        public decimal DistributionPerPerson { get; set; }
        public long Days { get; set; }
    }
}