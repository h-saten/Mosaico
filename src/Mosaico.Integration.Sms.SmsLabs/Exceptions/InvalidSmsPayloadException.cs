using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Integration.Sms.SmsLabs.Exceptions
{
    public class InvalidSmsPayloadException : ExceptionBase
    {
        public InvalidSmsPayloadException() : base($"Invalid sms payload")
        {
        }

        public override string Code => Constants.ErrorCodes.InvalidSmsPayload;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}