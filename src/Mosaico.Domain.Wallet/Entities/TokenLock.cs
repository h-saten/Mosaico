using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class TokenLock : EntityBase
    {
        public decimal Amount { get; set; }
        public bool Expired { get; set; }
        public string UserId { get; set; }
        public Guid? TokenId { get; set; }
        public virtual Token Token { get; set; }
        public virtual PaymentCurrency PaymentCurrency { get; set; }
        public Guid? PaymentCurrencyId { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
        public string LockReason { get; set; }
    }
}