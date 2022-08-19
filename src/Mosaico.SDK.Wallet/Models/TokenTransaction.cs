using System;

namespace Mosaico.SDK.Wallet.Models
{
    public class TokenTransaction
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public string TransactionHash { get; set; }
        public string FinishedAt { get; set; }
        public string PaymentProcessor { get; set; }
    }
}