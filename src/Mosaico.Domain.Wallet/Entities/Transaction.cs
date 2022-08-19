using System;
using Mosaico.Domain.Base;
using Mosaico.Domain.Identity.ValueObjects;

namespace Mosaico.Domain.Wallet.Entities
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
        public string Currency { get; set; }
        public virtual PaymentCurrency PaymentCurrency { get; set; }
        public Guid? PaymentCurrencyId { get; set; }
        public virtual TransactionStatus Status { get; set; }
        public Guid StatusId { get; set; }
        public DateTimeOffset InitiatedAt { get; set; }
        public DateTimeOffset? FinishedAt { get; set; }
        public string FailureReason { get; set; }
        public virtual TransactionType Type { get; set; }
        public Guid TypeId { get; set; }
        public Guid? TokenId { get; set; }
        public Guid? StageId { get; set; }
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
        public Guid? ProjectId { get; set; }
        public decimal? TokenPrice { get; set; }
        public decimal? GasFee { get; set; }
        public decimal? FeeInUSD { get; set; }
        public decimal? MosaicoFee { get; set; }
        public decimal? MosaicoFeeInUSD { get; set; }
        public decimal FeePercentage { get; set; }
        public decimal? ExchangeRate { get; set; }
        
        public Guid? SalesAgentId { get; set; }
        public virtual SalesAgent SalesAgent { get; set; }
        public void SetStatus(TransactionStatus status)
        {
            Status = status;
            StatusId = status.Id;
        }

        public void SetType(TransactionType type)
        {
            Type = type;
            TypeId = type.Id;
        }
        
        public void IncreaseConfirmationAttemptsCounter()
        {
            if (LastConfirmationAttemptedAt == null)
            {
                NextConfirmationAttemptAt = DateTimeOffset.UtcNow.AddSeconds(60);
            }
            else
            {
                NextConfirmationAttemptAt = DateTimeOffset.UtcNow.AddSeconds(ConfirmationAttemptsCounter * 60 * Math.Pow(2, ConfirmationAttemptsCounter));
            }
            ConfirmationAttemptsCounter += 1;
            LastConfirmationAttemptedAt = DateTimeOffset.UtcNow;
        }
    }
}