using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class TokenNotMintableException : ExceptionBase
    {
        public TokenNotMintableException(Guid id) : base($"Token {id} is not mintable")
        {
        }

        public override string Code => Constants.ErrorCodes.TokenNotMintable;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}