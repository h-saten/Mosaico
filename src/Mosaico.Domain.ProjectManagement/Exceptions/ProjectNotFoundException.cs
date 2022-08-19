using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class ProjectNotFoundException : ExceptionBase
    {
        public ProjectNotFoundException(string name) : base($"Project {name} not found")
        {
        }

        public ProjectNotFoundException(Guid id) : base($"Project with ID {id} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.ProjectNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}