using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class StakingAlreadyExistsException : ExceptionBase
    {
        public StakingAlreadyExistsException(string id) : base($"Staking {id} already exists")
        {
        }

        public override string Code => Constants.ErrorCodes.StakingAlreadyExists;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}