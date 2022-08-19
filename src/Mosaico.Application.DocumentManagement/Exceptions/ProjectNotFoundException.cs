using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.Exceptions
{
    public class ProjectNotFoundException : ExceptionBase
    {
        public ProjectNotFoundException(string projectId) : base($"Project {projectId} not found")
        { 
        }
        public override string Code => Constants.ErrorCodes.ProjectNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}
