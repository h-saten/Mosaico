using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Wallet.Exceptions
{
    public class TokenTypeNotFoundException : ExceptionBase
    {
        public TokenTypeNotFoundException(string id) : base($"Token type {id} was not found")
        {
        }

        public override string Code => Constants.ErrorCodes.TokenTypeNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}