using System;

namespace Mosaico.Application.Wallet.DTO
{
    public class TokenBalanceDTO
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
        public string ContractAddress { get; set; }
        public string LogoUrl { get; set; }
        public bool IsExchangable { get; set; }
        public bool IsStakable { get; set; }
        public decimal TotalAssetValue { get; set; }
        public string Currency { get; set; }
        public bool IsPaymentCurrency { get; set; }
        public string Network { get; set; }
    }
}