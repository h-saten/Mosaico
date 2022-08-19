using System;
using Mosaico.Events.Wallet;

namespace Mosaico.Application.Wallet.Tests.Factories.Events
{
    internal static class TransactionInitiatedEventFactory
    {
        public static TransactionInitiatedEvent Create(Guid transactionId, string walletAccountAddress)
        {
            var eventPayload = new TransactionInitiatedEvent
            {
                PaymentProcessor = "transak",
                ProjectId = Guid.NewGuid(),
                TokenAmount = 1000,
                TransactionId = transactionId,
                WalletAddress = walletAccountAddress
            };
            return eventPayload;
        }
        
        public static TransactionInitiatedEvent Create() 
            => Create(Guid.NewGuid(), EthereumAddressFaker.Generate());
        
        public static TransactionInitiatedEvent Create(Guid transactionId) 
            => Create(transactionId, EthereumAddressFaker.Generate());
    }
}