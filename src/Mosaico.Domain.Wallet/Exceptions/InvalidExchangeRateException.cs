using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Wallet.Exceptions
{
    public class InvalidExchangeRateException : ExceptionBase
    {
        public InvalidExchangeRateException(string message) : base($"{message} invalid exchange rate")
        {
        }

        public override string Code => "INVALID_EXCHANGE_RATE";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}