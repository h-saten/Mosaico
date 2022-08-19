using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class VestingAlreadyExistsException : ExceptionBase
    {
        public VestingAlreadyExistsException(string id) : base($"Vesting {id} already exists")
        {
        }

        public override string Code => Constants.ErrorCodes.VestingAlreadyExists;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}