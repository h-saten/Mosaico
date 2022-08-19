using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class NothingToClaimException : ExceptionBase
    {
        public NothingToClaimException(string userId, Guid pairId) : base($"User {userId} has nothing to claim from pair {pairId}")
        {
        }

        public override string Code => "NOTHING_TO_CLAIM";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}