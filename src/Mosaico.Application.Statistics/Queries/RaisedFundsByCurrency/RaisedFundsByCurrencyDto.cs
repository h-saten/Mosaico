namespace Mosaico.Application.Statistics.Queries.RaisedFundsByCurrency
{
    public class RaisedFundsByCurrencyDto
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public decimal UsdtAmount { get; set; }
    }
}