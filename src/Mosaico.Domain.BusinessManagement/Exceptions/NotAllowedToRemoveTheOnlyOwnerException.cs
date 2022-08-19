using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;
using System;

namespace Mosaico.Domain.BusinessManagement.Exceptions
{
    public class NotAllowedToRemoveTheOnlyOwnerException : ExceptionBase
    {
        public NotAllowedToRemoveTheOnlyOwnerException() : base($"Not allowed to remove the only owner")
        {
        }

        public override string Code => Constants.ErrorCodes.NotAllowedToRemoveTheOnlyOwner;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}