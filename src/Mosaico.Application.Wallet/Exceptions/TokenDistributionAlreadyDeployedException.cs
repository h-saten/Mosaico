using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class TokenDistributionAlreadyDeployedException : ExceptionBase
    {
        public TokenDistributionAlreadyDeployedException(Guid id) : base($"Token distribution {id} already deployed")
        {
        }

        public override string Code => "TOKEN_DISTRIBUTION_ALREADY_DEPLOYED";
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}