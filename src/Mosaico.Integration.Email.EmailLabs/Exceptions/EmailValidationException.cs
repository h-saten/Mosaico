using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Integration.Email.EmailLabs.Exceptions
{
    public class EmailValidationException : ExceptionBase
    {
        public EmailValidationException(string message) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.EmailValidationError;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}