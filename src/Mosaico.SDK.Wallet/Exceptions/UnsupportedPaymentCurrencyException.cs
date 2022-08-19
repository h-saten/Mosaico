using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.SDK.Wallet.Exceptions
{
    public class UnsupportedPaymentCurrencyException : ExceptionBase
    {
        public string Symbol { get; set; }
        public UnsupportedPaymentCurrencyException() : base($"Payment currency was not found.")
        {
        }
        public UnsupportedPaymentCurrencyException(string symbol) : base($"Payment currency: '{symbol}' was not found.")
        {
            Symbol = symbol;
        }

        public override string Code => "UNSUPPORTED_PAYMENT_CURRENCY";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}