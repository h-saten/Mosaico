using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Domain.Identity.Exceptions
{
    public class PhoneNumberConfirmationCodeInvalidException : ExceptionBase
    {
        public PhoneNumberConfirmationCodeInvalidException() : base($"Phone number confirmation code is invalid")
        {
        }

        public override string Code => Constants.ErrorCodes.PhoneNumberConfirmationCodeInvalid;
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}