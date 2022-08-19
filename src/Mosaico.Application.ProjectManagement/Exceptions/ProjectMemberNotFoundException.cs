using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class ProjectMemberNotFoundException : ExceptionBase
    {
        public ProjectMemberNotFoundException(Guid id) : base($"Project member {id} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.ProjectMemberNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}