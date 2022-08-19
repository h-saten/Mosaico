using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class StakingAlreadyEnabledException : ExceptionBase
    {
        public StakingAlreadyEnabledException(Guid tokenId) : base($"Staking for token {tokenId} already exists")
        {
        }

        public override string Code => Constants.ErrorCodes.StakingAlreadyEnabled;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}