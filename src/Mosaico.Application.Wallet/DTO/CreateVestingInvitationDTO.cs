using System;

namespace Mosaico.Application.Wallet.DTO
{
    public class CreateVestingInvitationDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string WalletAddress { get; set; }
    }
}