using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class InvitationAlreadyExpiredException : ExceptionBase
    {
        public InvitationAlreadyExpiredException(string id) : base($"Invitation {id} already expired")
        {
        }

        public override string Code => Constants.ErrorCodes.InvitationAlreadyExpired;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}