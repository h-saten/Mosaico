using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Identity.Exceptions
{
    public class SecurityCodeInvalidException : ExceptionBase
    {
        public SecurityCodeInvalidException() : base($"Security code is invalid")
        {
        }

        public override string Code => Constants.ErrorCodes.SecurityCodeInvalid;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}