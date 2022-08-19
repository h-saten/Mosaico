namespace Mosaico.Application.Statistics.Queries.StatisticsSummary
{
    public class StatisticsSummaryResponse
    {
        public int TokenPageVisits { get; set; }
        public int FundPageVisits { get; set; }
        public int TransactionsCounter { get; set; }
        public int Followers { get; set; }
        public decimal AverageTransaction { get; set; }
        public decimal SmallestTransactionAmount { get; set; }
        public decimal HighestTransactionAmount { get; set; }
        public decimal MedianTransactionAmount { get; set; }
        public decimal RaisedFunds { get; set; }
    }
}