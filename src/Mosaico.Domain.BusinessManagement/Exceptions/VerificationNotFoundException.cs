using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class VerificationNotFoundException : ExceptionBase
    {
        public VerificationNotFoundException(string name) : base($"Verification {name} was not found")
        {
        }

        public VerificationNotFoundException(Guid id) : base($"Verification with id {id} was not found")
        {
        }

        public override string Code => Constants.ErrorCodes.VerificationNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}