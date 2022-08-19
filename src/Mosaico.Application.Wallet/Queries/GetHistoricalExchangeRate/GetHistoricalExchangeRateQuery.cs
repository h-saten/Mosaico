using System;
using MediatR;

namespace Mosaico.Application.Wallet.Queries.GetHistoricalExchangeRate
{
    public class GetHistoricalExchangeRateQuery : IRequest<GetHistoricalExchangeRateQueryResponse>
    {
        public string Symbol { get; set; }
        public DateTimeOffset? From { get; set; }
        public DateTimeOffset? To { get; set; }
    }
}