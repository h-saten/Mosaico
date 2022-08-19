using System.Collections.Generic;
using Mosaico.Application.Wallet.DTO;

namespace Mosaico.Application.Wallet.Queries.Checkout
{
    public abstract class CheckoutResponseBase
    {
        public List<ExchangeRateDTO> ExchangeRates { get; set; } = new List<ExchangeRateDTO>();
        public string ProjectName { get; set; }
        public string CompanyName { get; set; }
        public string RegulationsUrl { get; set; }
        public string PrivacyPolicyUrl { get; set; }
        public decimal MinimumPurchase { get; set; }
        public decimal MaximumPurchase { get; set; }
        public List<PaymentCurrencyDTO> Currencies { get; set; } = new List<PaymentCurrencyDTO>();
    }
}