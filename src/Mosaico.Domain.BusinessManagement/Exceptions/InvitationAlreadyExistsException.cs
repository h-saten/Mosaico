using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class InvitationAlreadyExistsException : ExceptionBase
    {
        public InvitationAlreadyExistsException(string name) : base($"Invitation for email {name} already exists")
        {
        }

        public override string Code => Constants.ErrorCodes.InvitationAlreadyExists;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}