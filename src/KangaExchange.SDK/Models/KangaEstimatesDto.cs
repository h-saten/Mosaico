using System;
using System.Data;

namespace KangaExchange.SDK.Models
{
    public class KangaEstimatesDto
    {
        public KangaPaymentCurrencyDto BTC { get; set; }
        public KangaPaymentCurrencyDto ETH { get; set; }
        public KangaPaymentCurrencyDto oPLN { get; set; }
        public KangaPaymentCurrencyDto oEUR { get; set; }
        public KangaPaymentCurrencyDto oUSD { get; set; }
        public KangaPaymentCurrencyDto USDT { get; set; }

        private KangaPaymentCurrencyDto GetForCurrency(KangaPaymentCurrencyEnumDto currency)
        {
            if (currency == KangaPaymentCurrencyEnumDto.PLN)
            {
                return oPLN;
            }
            if (currency == KangaPaymentCurrencyEnumDto.BTC)
            {
                return BTC;
            }
            if (currency == KangaPaymentCurrencyEnumDto.ETH)
            {
                return ETH;
            }
            if (currency == KangaPaymentCurrencyEnumDto.EUR)
            {
                return oEUR;
            }
            if (currency == KangaPaymentCurrencyEnumDto.USD)
            {
                return oUSD;
            }
            if (currency == KangaPaymentCurrencyEnumDto.USDT)
            {
                return USDT;
            }
            
            throw new DataException("estimates_not_found");
        }
        
        public decimal CalculateTokensAmountForCurrency
            (decimal currencyAmount, KangaPaymentCurrencyEnumDto currency)
        {
            var estimatesForSpecifiedCurrency = GetForCurrency(currency);

            var tokensAmount = estimatesForSpecifiedCurrency.TokensAmount * currencyAmount 
                               / estimatesForSpecifiedCurrency.CurrencyAmount;

            return tokensAmount;
        }
        
        public decimal CalculateToSelectedCurrency
            (KangaPaymentCurrencyEnumDto fromCurrency, decimal currencyAmount, KangaPaymentCurrencyEnumDto toCurrency)
        {
            if (fromCurrency == toCurrency)
            {
                return currencyAmount;
            }

            var tokensAmountInInputCurrency = CalculateTokensAmountForCurrency(currencyAmount, fromCurrency);
            
            var estimatesForOutputCurrency = GetForCurrency(toCurrency);

            var outputTokensAmount = Math.Round(tokensAmountInInputCurrency * estimatesForOutputCurrency.CurrencyAmount 
                               / estimatesForOutputCurrency.TokensAmount, 4);

            return outputTokensAmount;
        }
    }
}