using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class ProjectAlreadyExistsException : ExceptionBase
    {
        public ProjectAlreadyExistsException(string name) : base($"Project {name} already exist")
        {
        }

        public override string Code => Constants.ErrorCodes.ProjectAlreadyExists;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}