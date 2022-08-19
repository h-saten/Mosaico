using System;
using System.Collections.Generic;

namespace Mosaico.Application.Wallet.DTO
{
    public class TokenPriceHistoryItem
    {
        public DateTimeOffset? Date { get; set; }
        public decimal Rate { get; set; }
    }
    
    public class TokenPriceHistoryDTO
    {
        public Guid? TokenId { get; set; }
        public string TokenName { get; set; }
        public string TokenSymbol { get; set; }
        public string Currency { get; set; }
        public decimal LatestPrice { get; set; }
        public string Logo { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalValue { get; set; }
        public bool IsStakingEnabled { get; set; }
        public List<TokenPriceHistoryItem> Records { get; set; } = new List<TokenPriceHistoryItem>();
    }
}