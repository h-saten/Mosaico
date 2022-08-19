using System;

namespace Mosaico.Events.Wallet
{
    public record WalletCreatedEvent
    {
        public Guid WalletId { get; init; }
    }
}