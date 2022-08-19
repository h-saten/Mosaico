using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.ProjectManagement.Exceptions
{
    public class InvitationNotFoundException : ExceptionBase
    {
        public InvitationNotFoundException(string id) : base($"Invitation with id {id} not found")
        {
        }

        public override string Code => Constants.ErrorCodes.InvitationNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}