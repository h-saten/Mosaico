using System;

namespace Mosaico.Events.Wallet
{
    public record TransactionConfirmedEvent
    {
        public string Currency { get; init; }
        public decimal Payed { get; init; }
        public Guid TransactionId { get; init; }
    }
}