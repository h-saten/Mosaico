using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class StakingNotFoundException : ExceptionBase
    {
        public StakingNotFoundException() : base($"Staking was not found")
        {
        }
        public StakingNotFoundException(Guid id) : base($"Staking with id {id} was not found")
        {
        }
        
        public StakingNotFoundException(string id) : base($"Staking with id {id} was not found")
        {
        }

        public override string Code => Constants.ErrorCodes.StakingNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}