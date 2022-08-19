using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Wallet.Exceptions
{
    public class ProjectTokenNotFoundException : ExceptionBase
    {
        public ProjectTokenNotFoundException(Guid id) : base($"Token for project {id} was not found")
        {
        }

        public override string Code => Constants.ErrorCodes.TokenNotFound;
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}