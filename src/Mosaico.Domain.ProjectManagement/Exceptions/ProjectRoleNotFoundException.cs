using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class ProjectRoleNotFoundException : ExceptionBase
    {
        public ProjectRoleNotFoundException(string role) : base($"Role {role} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.RoleNotFound;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}