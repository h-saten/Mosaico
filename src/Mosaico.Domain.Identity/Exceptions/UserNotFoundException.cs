using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Identity.Exceptions
{
    public class UserNotFoundException : ExceptionBase
    {
        public UserNotFoundException(string id) : base($"User with ID {id} not found")
        {
            ExtraData = new
            {
                id
            };
        }

        public UserNotFoundException(Guid id) : base($"User with ID {id} not found")
        {
            ExtraData = new
            {
                id
            };
        }

        public override string Code => Constants.ErrorCodes.UserNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}