using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Identity.Exceptions
{
    public class RegistrationFailedException : ExceptionBase
    {
        public RegistrationFailedException(string message = null) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.RegistrationFailed;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}