using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Services.Abstractions;

namespace Mosaico.Application.Wallet.Queries.GetExchangeRates
{
    public class GetExchangeRatesQueryHandler : IRequestHandler<GetExchangeRatesQuery, GetExchangeRatesQueryResponse>
    {
        private readonly IExchangeRateService _exchangeRate;
        
        public GetExchangeRatesQueryHandler(IExchangeRateService exchangeRate)
        {
            _exchangeRate = exchangeRate;
        }

        public async Task<GetExchangeRatesQueryResponse> Handle(GetExchangeRatesQuery request, CancellationToken cancellationToken)
        {
            var rates = await _exchangeRate.GetExchangeRatesAsync();
            return new GetExchangeRatesQueryResponse
            {
                ExchangeRates = rates.Select(er => new ExchangeRateDTO
                {
                    Currency = er.Ticker,
                    BaseCurrency = er.BaseCurrency,
                    ExchangeRate = er.Rate
                }).ToList()
            };
        }
    }
}