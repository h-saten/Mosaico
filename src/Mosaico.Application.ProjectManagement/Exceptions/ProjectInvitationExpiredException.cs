using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class ProjectInvitationExpiredException : ExceptionBase
    {
        public ProjectInvitationExpiredException() : base($"Invitation already expired")
        {
        }

        public override string Code => Constants.ErrorCodes.ProjectInvitationExpired;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}