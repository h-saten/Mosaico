using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class UnsupportedCurrencyException : ExceptionBase
    {
        public string Currency { get; set; }
        
        public UnsupportedCurrencyException(string currency) : base($"Payment currency: '{currency}' is not supported.")
        {
            Currency = currency;
        }

        public override string Code => Constants.ErrorCodes.UnsupportedCurrency;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}