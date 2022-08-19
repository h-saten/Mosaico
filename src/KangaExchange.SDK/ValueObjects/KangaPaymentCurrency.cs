using System.Collections.Generic;
using KangaExchange.SDK.Exceptions;

namespace KangaExchange.SDK.ValueObjects
{
    public class KangaPaymentCurrency
    {
        public static readonly string BTC = "BTC";
        public static readonly string ETH = "ETH";
        public static readonly string PLN = "oPLN";
        public static readonly string EUR = "oEUR";
        public static readonly string USD = "oUSD";
        public static readonly string USDT = "USDT";
        public static readonly string USDC = "USDC";
        
        private readonly List<string> _supportedCurrencies = new()
        {
            "BTC", "ETH", "oPLN", "oEUR", "oUSD", "USDT", "USDC"
        };
        
        private string _currency;

        public KangaPaymentCurrency(string currency)
        {
            if (_supportedCurrencies.Contains(currency) is false)
            {
                throw new UnsupportedKangaCurrencyException(currency);
            }
            _currency = currency;
        }

        public string SupportedTransactionType()
        {
            if (_currency == "oPLN" || _currency == "oEUR" || _currency == "oUSD")
            {
                return "FIAT";
            }

            return "ON-CHAIN";
        }

        public string GlobalCurrencySymbol()
        {
            return _currency.TrimStart('o');
        }

        public override string ToString() => _currency;
    }
}