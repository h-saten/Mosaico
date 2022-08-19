using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Identity.Exceptions
{
    public class IncorrectPasswordException : ExceptionBase
    {
        public IncorrectPasswordException() : base($"Incorrect user credentials")
        {
        }

        public override string Code => Constants.ErrorCodes.IncorrectPassword;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}