using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class StakingIsDisabledException : ExceptionBase
    {
        public StakingIsDisabledException(Guid stakingId) : base($"Staking {stakingId} is disabled")
        {
        }

        public override string Code => "STAKING_IS_DISABLED";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}