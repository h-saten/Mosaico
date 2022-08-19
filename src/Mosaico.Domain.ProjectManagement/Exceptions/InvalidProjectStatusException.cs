using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class InvalidProjectStatusException : ExceptionBase
    {
        public InvalidProjectStatusException(string message) : base($"Project {message} is in invalid state")
        {
        }

        public override string Code => Constants.ErrorCodes.InvalidProjectStatus;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}