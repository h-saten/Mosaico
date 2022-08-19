using System;

namespace Mosaico.Integration.SignalR.DTO
{
    public class DeploymentEstimateDTO
    {
        public decimal Gas { get; set; }
        public string ContractVersion { get; set; }
        public decimal GasPrice { get; set; }
        public decimal Price { get; set; }
        public string Network { get; set; }
        public string Currency { get; set; }
        public DateTimeOffset EstimatedAt { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Fee { get; set; }
        public string NativeCurrencyTicker { get; set; }
        public decimal NativeCurrencyAmount { get; set; }
    }
}