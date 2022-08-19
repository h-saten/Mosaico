using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Identity.Exceptions
{
    public class LoginFailedException : ExceptionBase
    {
        public LoginFailedException(string message) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.LoginFailed;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}