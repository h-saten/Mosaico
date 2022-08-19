using System;
using System.Collections.Generic;

namespace Mosaico.Application.Wallet.DTO
{
    public class CreateVestingFundDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Distribution { get; set; }
        public long Days { get; set; }
        public DateTimeOffset? StartAt { get; set; }
        public DateTimeOffset? EndAt { get; set; }
        public bool CanWithdrawEarly { get; set; }
        public long? SubtractedPercent { get; set; }
        public List<CreateVestingInvitationDTO> Invitations { get; set; }
        public Guid? StageId { get; set; }
    }
}