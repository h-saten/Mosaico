using System;

namespace Mosaico.Application.Wallet.DTO
{
    public class VestingInvitationDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string WalletAddress { get; set; }
        public bool IsAccepted { get; set; }
    }
}