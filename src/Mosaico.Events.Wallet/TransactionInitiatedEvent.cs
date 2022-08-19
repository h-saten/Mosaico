using System;

namespace Mosaico.Events.Wallet
{
    public record TransactionInitiatedEvent
    {
        public Guid? ProjectId { get; init; }
        public decimal? TokenAmount { get; init; }
        public Guid TransactionId { get; init; }
        public string PaymentProcessor { get; init; }
        public string WalletAddress { get; init; }
    }
}