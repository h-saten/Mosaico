using System.Collections.Generic;
using MediatR;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.GetPaymentCurrencies
{
    [Cache("{{Network}}")]
    public class GetPaymentCurrenciesQuery : IRequest<List<PaymentCurrencyDTO>>
    {
        public string Network { get; set; }
    }
}