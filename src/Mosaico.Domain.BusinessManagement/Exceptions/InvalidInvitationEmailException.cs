using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class InvalidInvitationEmailException : ExceptionBase
    {
        public InvalidInvitationEmailException(string userId) : base($"User {userId} is not eligable to accept this invitation")
        {
        }

        public override string Code => Constants.ErrorCodes.UserNotAuthorizedToAcceptInvitation;
        public override int StatusCode => StatusCodes.Status403Forbidden;
    }
}