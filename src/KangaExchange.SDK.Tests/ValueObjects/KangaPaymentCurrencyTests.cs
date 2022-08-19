using KangaExchange.SDK.Exceptions;
using KangaExchange.SDK.ValueObjects;
using NUnit.Framework;

namespace KangaExchange.SDK.Tests.ValueObjects
{
    public class KangaPaymentCurrencyTests
    {
        [TestCase("oPLN", "PLN")]
        [TestCase("oEUR", "EUR")]
        [TestCase("BTC", "BTC")]
        public void ShouldReturnValidGlobalSymbol(string given, string expected)
        {
            Assert.AreEqual(expected, new KangaPaymentCurrency(given).GlobalCurrencySymbol());
        }

        [TestCase("OPLN")]
        [TestCase("EUR")]
        [TestCase("eur")]
        public void ShouldThrowExceptionWhenCurrencySymbolIsUnsupported(string given)
        {
            Assert.Catch<UnsupportedKangaCurrencyException>(() => new KangaPaymentCurrency(given));
        }
    }
}