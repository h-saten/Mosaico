using System;

namespace Mosaico.Application.Wallet.DTO
{
    public class PaymentCurrencyDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public string ContractAddress { get; set; }
        public bool NativeChainCurrency { get; set; }
        public string Network { get; set; }
        public int Decimals { get; set; }
        public string LogoUrl { get; set; }
    }
}