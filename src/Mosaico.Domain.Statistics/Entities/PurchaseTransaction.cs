using System;

namespace Mosaico.Domain.Statistics.Entities
{
    public class PurchaseTransaction
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; init; }
        public Guid TokenId { get; init; }
        public Guid TransactionId { get; init; }
        public string Currency { get; init; }
        public decimal Payed { get; init; }
        public decimal TokensAmount { get; init; }
        public decimal USDTAmount { get; init; }
        public DateTimeOffset Date { get; init; }
    }
}