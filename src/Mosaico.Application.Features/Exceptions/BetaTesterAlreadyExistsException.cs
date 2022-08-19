using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Features.Exceptions
{
    public class BetaTesterAlreadyExistsException : ExceptionBase
    {
        public BetaTesterAlreadyExistsException(string id) : base($"User {id} is already a beta tester")
        {
        }

        public override string Code => Constants.ErrorCodes.BetaTesterAlreadyExists;
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}