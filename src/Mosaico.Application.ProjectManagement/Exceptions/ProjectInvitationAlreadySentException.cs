using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class ProjectInvitationAlreadySentException : ExceptionBase
    {
        public ProjectInvitationAlreadySentException(string email) : base($"User with email {email} was already invited")
        {
        }

        public override string Code => Constants.ErrorCodes.ProjectInvitationAlreadySent;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}