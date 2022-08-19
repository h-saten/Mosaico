using System.Collections.Generic;
using Mosaico.Application.Wallet.DTO;

namespace Mosaico.Application.Wallet.Queries.Checkout.MetamaskCheckoutContext
{
    public class MetamaskCheckoutContextQueryResponse
    {
        public List<ExchangeRateDTO> ExchangeRates { get; set; } = new List<ExchangeRateDTO>();
        public string PaymentAddress { get; set; }
        public string ProjectName { get; set; }
        public string CompanyName { get; set; }
        public string RegulationsUrl { get; set; }
        public string PrivacyPolicyUrl { get; set; }
        public List<PaymentCurrencyDTO> Currencies { get; set; } = new List<PaymentCurrencyDTO>();
        public decimal MinimumPurchase { get; set; }
        public decimal MaximumPurchase { get; set; }
    }
}