using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class AnotherStakingInProgressException : ExceptionBase
    {
        public AnotherStakingInProgressException(string userId) : base($"User {userId} has another staking in progress")
        {
        }

        public override string Code => "STAKING_IN_PROGRESS";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}