using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class ProjectStatusNotFoundException : ExceptionBase
    {
        public ProjectStatusNotFoundException(string message) : base($"Project status {message} was not found")
        {
        }

        public override string Code => "PROJECT_STATUS_NOT_FOUND";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}