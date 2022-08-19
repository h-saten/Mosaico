using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions
{
    public class NoInvitationsToSendException : ExceptionBase
    {
        public NoInvitationsToSendException() : base($"There were no invitations to send")
        {
        }

        public override string Code => Constants.ErrorCodes.NoInvitations;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}