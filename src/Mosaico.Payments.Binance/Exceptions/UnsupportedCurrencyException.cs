using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Payments.Binance.Exceptions
{
    public class UnsupportedCurrencyException : ExceptionBase
    {
        public UnsupportedCurrencyException(string message) : base($"Binance does not support {message} currency")
        {
        }

        public override string Code => "UNSUPPORTED_BINANCE_CURRENCY";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}