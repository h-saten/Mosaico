using System;

namespace Mosaico.Events.Wallet
{
    public record PurchaseTransactionConfirmedEvent
    {
        public string UserId { get; init; }
        public Guid TokenId { get; init; }
        public Guid TransactionId { get; init; }
        public string TransactionCorrelationId { get; set; }
        public string Currency { get; init; }
        public decimal Payed { get; init; }
        public decimal TokensAmount { get; init; }
        public Guid ProjectId { get; set; }
        public string RefCode { get; set; }
    }
}