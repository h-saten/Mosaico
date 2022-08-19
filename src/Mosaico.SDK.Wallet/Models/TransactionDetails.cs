using System;

namespace Mosaico.SDK.Wallet.Models
{
    public class TransactionDetails
    {
        public decimal? TokenAmount { get; set; }
        public decimal? PayedAmount { get; set; }
        public string UserId { get; set; }
        public string Currency { get; set; }
        public DateTimeOffset InitiatedAt { get; set; }
        public DateTimeOffset? FinishedAt { get; set; }
        public Guid? TokenId { get; set; }
    }
}