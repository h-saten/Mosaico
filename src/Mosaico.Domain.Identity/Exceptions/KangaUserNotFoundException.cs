
using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Identity.Exceptions
{
    public class KangaUserNotFoundException : ExceptionBase
    {
        public KangaUserNotFoundException(string id) : base($"User with ID {id} not found")
        {
            ExtraData = new
            {
                id
            };
        }

        public KangaUserNotFoundException(Guid id) : base($"User with ID {id} not found")
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