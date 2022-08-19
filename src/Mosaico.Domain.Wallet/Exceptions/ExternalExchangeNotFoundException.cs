using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Wallet.Exceptions
{
    public class ExternalExchangeNotFoundException : ExceptionBase
    {
        public ExternalExchangeNotFoundException(string message) : base($"External exchange {message} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.ExternalExchangeNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}