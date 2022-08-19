using System;

namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class ProjectTransactionDTO
    {
        public Guid TransactionId { get; set; }
        public string UserName { get; set; }
        public string TranHash { get; set; }
        public decimal TokenAmount { get; set; }
        public string TokenSymbol { get; set; }
        public string UserWallet { get; set; }
        public decimal PayedAmount { get; set; }
        public decimal? PayedInUSD { get; set; }
        public string Currency { get; set; }
        public string PaymentCurrencySymbol { get; set; }
        public DateTimeOffset? PurchasedDate { get; set; }
        public string Source { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public string FailureReason { get; set; }
        public string CorrelationId { get; set; }
        public string PaymentMethod { get; set; }
        public string ExtraData { get; set; }
        public string IntermediateAddress { get; set; }
        public decimal? TokenPrice { get; set; }
        public decimal? GasFee { get; set; }
        public decimal? Fee { get; set; }
        public decimal? FeeInUSD { get; set; }
        public decimal? MosaicoFee { get; set; }
        public decimal? MosaicoFeeInUSD { get; set; }
        public decimal FeePercentage { get; set; }
        public decimal? ExchangeRate { get; set; }
        public Guid? SalesAgentId { get; set; }
        public string ExternalLink { get; set; }
    }
}