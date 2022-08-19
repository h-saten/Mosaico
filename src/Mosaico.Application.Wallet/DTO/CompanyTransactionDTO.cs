using System;
using Mosaico.Domain.Wallet.Entities.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.Wallet.DTO
{
    public class CompanyTransactionDTO
    {
        public decimal TokenAmount { get; set; }
        public decimal? PayedAmount { get; set; }
        public string PaymentProcessor { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public DateTimeOffset InitiatedAt { get; set; }
        public DateTimeOffset? FinishedAt { get; set; }
        public string FailureReason { get; set; }
        public string TransactionType { get; set; }
        public Guid? TransactionId { get; set; }
        public string TransactionHash { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public TokenDTO Token { get; set; }
        public string FromDisplayName { get; set; }
        public string ToDisplayName { get; set; }
        public string PaymentMethod { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public TransactionDirection TransactionDirection { get; set; }
    }
}