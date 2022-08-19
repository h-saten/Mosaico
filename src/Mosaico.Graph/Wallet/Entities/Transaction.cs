using System;
using Mosaico.Domain.Mongodb.Base.Models;

namespace Mosaico.Graph.Wallet.Entities
{
    public class Transaction : EntityBase
    {
        public string CorrelationId { get; set; }
        public decimal? TokenAmount { get; set; }
        public decimal? PayedAmount { get; set; }
        public decimal? PayedInUSD { get; set; }
        public string PaymentProcessor { get; set; }
        public string PaymentMethod { get; set; }
        public string UserId { get; set; }
        public User.Entities.User User { get; set; }
        public string Currency { get; set; }
        public string PaymentCurrencyId { get; set; }
        public string PaymentCurrency { get; set; }
        public string StatusId { get; set; }
        public DateTimeOffset InitiatedAt { get; set; }
        public DateTimeOffset? FinishedAt { get; set; }
        public string FailureReason { get; set; }
        public string TypeId { get; set; }
        public string TokenId { get; set; }
        public string StageId { get; set; }
        public string WalletAddress { get; set; }
        public string Network { get; set; }
        public string TransactionHash { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int ConfirmationAttemptsCounter { get; set; }
        public DateTimeOffset? LastConfirmationAttemptedAt { get; set; }
        public DateTimeOffset? NextConfirmationAttemptAt { get; set; }
        public string ToDisplayName { get; set; }
        public string FromDisplayName { get; set; }
        public string ExtraData { get; set; }
        public string ExchangeTransactionHash { get; set; }
        public string RefCode { get; set; }
        public decimal Fee { get; set; }
        public string IntermediateAddress { get; set; }
        public string ProjectId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}