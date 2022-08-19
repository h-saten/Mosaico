using System.Collections.Generic;
using Mosaico.Application.Statistics.Queries.DailyRaisedCapital;

namespace Mosaico.Application.Statistics.Queries.RaisedFundsByCurrency
{
    public class RaisedFundsByCurrencyResponse
    {
        public List<RaisedFundsByCurrencyDto> Statistics { get; set; }
    }
}