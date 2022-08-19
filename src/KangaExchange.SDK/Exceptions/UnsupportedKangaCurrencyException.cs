using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace KangaExchange.SDK.Exceptions
{
    public class UnsupportedKangaCurrencyException : ExceptionBase
    {
        public string Currency { get; set; }
        
        public UnsupportedKangaCurrencyException(string currency) : base($"Currency {currency} is not supported by Kanga.")
        {
            Currency = currency;
        }

        public override string Code => Constants.ErrorCodes.UnsupportedCurrency;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}