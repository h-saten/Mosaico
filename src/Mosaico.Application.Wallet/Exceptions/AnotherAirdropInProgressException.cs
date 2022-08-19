using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Wallet.Exceptions
{
    public class AnotherAirdropInProgressException : ExceptionBase
    {
        public AnotherAirdropInProgressException(Guid airdropId) : base($"Another operation for airdrop {airdropId} is in progress")
        {
        }

        public override string Code => "ANOTHER_AIRDROP_DISTRIBUTION_IN_PROGRESS";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}