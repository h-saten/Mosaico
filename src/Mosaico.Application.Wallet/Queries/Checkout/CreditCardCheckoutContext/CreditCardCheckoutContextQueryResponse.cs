using System.Collections.Generic;
using Mosaico.Application.Wallet.DTO;

namespace Mosaico.Application.Wallet.Queries.Checkout.CreditCardCheckoutContext
{
    public class CreditCardCheckoutContextQueryResponse : CheckoutResponseBase
    {
        public string PaymentAddress { get; set; }
    }
}