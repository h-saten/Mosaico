using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class InvitationNotFoundException : ExceptionBase
    {
        public InvitationNotFoundException(string id) : base($"Invitation with id {id} was not found")
        {
        }

        public InvitationNotFoundException(Guid id) : base($"Invitation with id {id} was not found")
        {
        }

        public override string Code => Constants.ErrorCodes.InvitationNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}