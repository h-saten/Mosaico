using System;

namespace Mosaico.SDK.Wallet.Models
{
    public class InvestorTokenSummary
    {
        public string TokenSymbol { get; set; }
        public decimal PaidTokensAmount { get; set; }
        public int InvestorSequenceNumber { get; set; }
        public int InvestorFinalizedTransactions { get; set; }
        public DateTimeOffset? LastTransactionDate { get; set; }
    }
}