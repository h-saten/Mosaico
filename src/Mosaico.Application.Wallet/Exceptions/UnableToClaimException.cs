using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class UnableToClaimException : ExceptionBase
    {
        public UnableToClaimException(string userId, Guid stakingPairId) : base($"User {userId} cannot claim {stakingPairId} at this cycle")
        {
        }

        public override string Code => "UNABLE_TO_CLAIM";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}