using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class ProjectBankTransferTitle : EntityBase
    {
        public string UserId { get; set; }
        public string Reference { get; set; }
        public Guid ProjectBankPaymentDetailsId { get; set; }
        public virtual ProjectBankPaymentDetails ProjectBankPaymentDetails { get; set; }
        public string Currency { get; set; }
        public decimal TokenAmount { get; set; }
        public decimal FiatAmount { get; set; }
    }
}