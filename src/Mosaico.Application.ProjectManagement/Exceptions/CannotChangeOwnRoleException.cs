using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class CannotChangeOwnRoleException : ExceptionBase
    {
        public CannotChangeOwnRoleException() : base($"Cannot change own role")
        {
        }

        public override string Code => Constants.ErrorCodes.CannotChangeOwnRole;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}