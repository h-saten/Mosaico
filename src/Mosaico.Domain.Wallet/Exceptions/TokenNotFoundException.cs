using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Wallet.Exceptions
{
    public class TokenNotFoundException : ExceptionBase
    {
        public TokenNotFoundException() : base($"Token was not found")
        {
        }
        public TokenNotFoundException(string id) : base($"Token {id} was not found")
        {
        }
        
        public TokenNotFoundException(Guid id) : base($"Token {id} was not found")
        {
        }

        public override string Code => Constants.ErrorCodes.TokenNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}