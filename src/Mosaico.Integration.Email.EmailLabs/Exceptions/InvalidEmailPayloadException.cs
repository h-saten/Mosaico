using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Integration.Email.EmailLabs.Exceptions
{
    public class InvalidEmailPayloadException : ExceptionBase
    {
        public InvalidEmailPayloadException() : base($"Invalid email payload")
        {
        }

        public override string Code => Constants.ErrorCodes.InvalidEmailPayload;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}