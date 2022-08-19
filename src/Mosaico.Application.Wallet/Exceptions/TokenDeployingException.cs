using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class TokenDeployingException : ExceptionBase
    {
        public TokenDeployingException(Guid tokenId) : base($"Token {tokenId} is deploying")
        {
        }

        public override string Code => "TOKEN_DEPLOYING";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}