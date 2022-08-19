using KangaExchange.SDK.Abstractions;
using KangaExchange.SDK.Models;

namespace KangaExchange.SDK
{
    public class KangaEstimateProcessor : IKangaEstimateProcessor
    {
        public EstimatesResponseDto Process(EstimatesApiResponseDto apiResponseData)
        { 
            var estimates = new KangaEstimatesDto();
            var kangaEstimates = apiResponseData.Estimates;
            if (kangaEstimates != null)
            {
                if (kangaEstimates.BTC != null)
                {
                    var currency = kangaEstimates.BTC;
                    var estimate = new KangaPaymentCurrencyDto
                    {
                        Currency = KangaPaymentCurrencyEnumDto.BTC,
                        DisplayTicker = "BTC",
                        OriginalTicker = "BTC",
                        IsCryptoCurrency = true,
                        TokensAmount = currency.Value,
                        CurrencyAmount = currency.Quantity
                    };
                    estimates.BTC = estimate;
                }

                if (kangaEstimates.ETH != null)
                {
                    var currency = kangaEstimates.ETH;
                    var estimate = new KangaPaymentCurrencyDto
                    {
                        Currency = KangaPaymentCurrencyEnumDto.ETH,
                        DisplayTicker = "ETH",
                        OriginalTicker = "ETH",
                        IsCryptoCurrency = true,
                        TokensAmount = currency.Value,
                        CurrencyAmount = currency.Quantity
                    };
                    estimates.ETH = estimate;
                }

                if (kangaEstimates.oPLN != null)
                {
                    var currency = kangaEstimates.oPLN;
                    var estimate = new KangaPaymentCurrencyDto
                    {
                        Currency = KangaPaymentCurrencyEnumDto.PLN,
                        DisplayTicker = "PLN",
                        OriginalTicker = "oPLN",
                        IsCryptoCurrency = false,
                        TokensAmount = currency.Value,
                        CurrencyAmount = currency.Quantity
                    };
                    estimates.oPLN = estimate;
                }

                if (kangaEstimates.oEUR != null)
                {
                    var currency = kangaEstimates.oEUR;
                    var estimate = new KangaPaymentCurrencyDto
                    {
                        Currency = KangaPaymentCurrencyEnumDto.EUR,
                        DisplayTicker = "EUR",
                        OriginalTicker = "oEUR",
                        IsCryptoCurrency = false,
                        TokensAmount = currency.Value,
                        CurrencyAmount = currency.Quantity
                    };
                    estimates.oEUR = estimate;
                }

                if (kangaEstimates.oUSD != null)
                {
                    var currency = kangaEstimates.oUSD;
                    var estimate = new KangaPaymentCurrencyDto
                    {
                        Currency = KangaPaymentCurrencyEnumDto.USD,
                        DisplayTicker = "USD",
                        OriginalTicker = "oUSD",
                        IsCryptoCurrency = false,
                        TokensAmount = currency.Value,
                        CurrencyAmount = currency.Quantity
                    };
                    estimates.oUSD = estimate;
                }

                if (kangaEstimates.USDT != null)
                {
                    var currency = kangaEstimates.USDT;
                    var estimate = new KangaPaymentCurrencyDto
                    {
                        Currency = KangaPaymentCurrencyEnumDto.USDT,
                        DisplayTicker = "USDT",
                        OriginalTicker = "USDT",
                        IsCryptoCurrency = false,
                        TokensAmount = currency.Value,
                        CurrencyAmount = currency.Quantity
                    };
                    estimates.USDT = estimate;
                }
            }

            var response = new EstimatesResponseDto
            {
                Estimates = estimates,
                Result = apiResponseData.Result,
                BuyCodeRequired = apiResponseData.BuyCodeRequired
            };
            
            return response;
        }
    }
}