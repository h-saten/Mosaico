using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.Identity.Exceptions
{
    public class InvalidMFATokenException : ExceptionBase
    {
        public InvalidMFATokenException() : base($"MFA token is invalid")
        {
        }

        public override string Code => "INVALID_MFA_TOKEN";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}