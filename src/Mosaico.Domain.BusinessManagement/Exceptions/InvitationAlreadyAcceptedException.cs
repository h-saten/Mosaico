using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class InvitationAlreadyAcceptedException : ExceptionBase
    {
        public InvitationAlreadyAcceptedException(string id) : base($"Invitation {id} was already accepted")
        {
        }

        public override string Code => Constants.ErrorCodes.InvitationAlreadyAccepted;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}