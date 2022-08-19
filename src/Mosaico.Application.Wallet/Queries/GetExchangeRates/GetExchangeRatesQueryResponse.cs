using System.Collections.Generic;
using Mosaico.Application.Wallet.DTO;

namespace Mosaico.Application.Wallet.Queries.GetExchangeRates
{
    public class GetExchangeRatesQueryResponse
    {
        public List<ExchangeRateDTO> ExchangeRates { get; set; } = new List<ExchangeRateDTO>();
    }
}