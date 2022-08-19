using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class StakingPairNotFoundException : ExceptionBase
    {
        public StakingPairNotFoundException(Guid id) : base($"Staking pair {id} was not found")
        {
        }

        public override string Code => "STAKING_PAIR_NOT_FOUND";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}