using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.GetExchangeRates
{
    [Cache(ExpirationInMinutes = 2)]
    public class GetExchangeRatesQuery : IRequest<GetExchangeRatesQueryResponse>
    {
    }
}