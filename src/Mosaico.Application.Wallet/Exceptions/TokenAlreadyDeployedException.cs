using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class TokenAlreadyDeployedException : ExceptionBase
    {
        public TokenAlreadyDeployedException(Guid message) : base($"Token {message} already deployed")
        {
        }

        public override string Code => Constants.ErrorCodes.TokenAlreadyDeployed;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}