using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class ProjectPartnerNotFoundException : ExceptionBase
    {
        public ProjectPartnerNotFoundException(string name) : base($"Project partner {name} not found")
        {
        }

        public ProjectPartnerNotFoundException(Guid id) : base($"Project partner with ID {id} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.ProjectPartnerNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;    
    }
}
