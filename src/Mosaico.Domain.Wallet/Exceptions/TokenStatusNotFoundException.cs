using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Wallet.Exceptions
{
    public class TokenStatusNotFoundException : ExceptionBase
    {
        public TokenStatusNotFoundException(string status) : base($"Token status {status} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.TokenStatusNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}