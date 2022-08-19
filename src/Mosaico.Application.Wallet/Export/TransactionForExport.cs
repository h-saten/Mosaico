using CsvHelper.Configuration.Attributes;

namespace Mosaico.Application.Wallet.Export
{
    public class CsvTransaction
    {
        [Name("Id")]
        public string CorrelationId { get; set; }
        
        [Name("Investor")]
        public string User { get; set; }
        
        [Name("Email")]
        public string UserEmail { get; set; }
        
        [Name("Tokens")]
        public decimal TokenAmount { get; set; }
        
        [Name("Payed")]
        public decimal Payed { get; set; }
        
        [Name("Currency")]
        public string Currency { get; set; }
        
        [Name("Payed (USD)")]
        public decimal PayedInUSD { get; set; }
        
        [Name("Token Price (USD)")]
        public decimal TokenPrice { get; set; }
        
        [Name("Processor")]
        public string PaymentProcessor { get; set; }
        
        [Name("Payment Method")]
        public string PaymentMethod { get; set; }
        
        [Name("Date")]
        public string FinishedAt { get; set; }
        
        [Name("Reff. Code")]
        public string RefCode { get; set; }
        
        [Name("Transaction")]
        public string TransactionHash { get; set; }
        
        [Name("Status")]
        public string Status { get; set; }
        
        [Name("FeePercentage")]
        public decimal FeePercentage { get; set; }
        
        [Name("Processor fee")]
        public decimal PaymentProcessorFee { get; set; }
        
        [Name("Processor fee (USD)")]
        public decimal PaymentProcessorFeeInUSD { get; set; }
        
        [Name("Mosaico Fee")]
        public decimal MosaicoFee { get; set; }
        
        [Name("Mosaico Fee (USD)")]
        public decimal MosaicoFeeInUSD { get; set; }
        
        [Name("ExchangeRate")]
        public decimal ExchangeRate { get; set; }
        
        [Name("Salesman")]
        public string Salesman { get; set; }
    }
}