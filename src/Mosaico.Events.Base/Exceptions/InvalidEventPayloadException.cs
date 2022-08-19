using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Events.Base.Exceptions
{
    public class InvalidEventPayloadException : ExceptionBase
    {
        public InvalidEventPayloadException(string errorDetails) : base(
            $"Event payload has invalid payload. {errorDetails}")
        {
        }

        public override string Code => Constants.ErrorCodes.InvalidEventPayload;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}