using Mosaico.Domain.Base;

namespace Mosaico.Domain.Identity.Entities
{
    public class KycVerification : EntityBase
    {
        public string TransactionId { get; set; }
        public string ExtraData { get; set; }
        public string UserId { get; set; }
        public KycVerificationStatus Status { get; set; }
        public string Provider { get; set; }
    }
}