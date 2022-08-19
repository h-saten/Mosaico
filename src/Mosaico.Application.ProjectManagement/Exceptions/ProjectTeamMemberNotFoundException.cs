using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class ProjectTeamMemberNotFoundException : ExceptionBase
    {
        public ProjectTeamMemberNotFoundException(string name) : base($"Project team member {name} not found")
        {
        }

        public ProjectTeamMemberNotFoundException(Guid id) : base($"Project team member with ID {id} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.ProjectTeamMemberNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;    
    }
}
