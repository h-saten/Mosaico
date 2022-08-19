using System.Collections.Generic;

namespace Mosaico.Application.Wallet.Queries.TransactionFee
{
    
    public class TransactionFeeQueryResponse
    {
        public decimal? TotalAmount { get; set; }
        public string Currency { get; set; }
        public long TransactionCount { get; set; }
        public Dictionary<string, decimal> Fees { get; set; }
    }
}