using System;

namespace Mosaico.SDK.ProjectManagement.Models
{
    public class MosaicoPaymentCurrency
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string ContractAddress { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool IsNativeCurrency { get; set; }
    }
}