using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Integration.Email.EmailLabs.Exceptions
{
    public class EmailLabsDeliveryException : ExceptionBase
    {
        public EmailLabsDeliveryException(string message) : base(message)
        {
        }

        public override string Code => Constants.ErrorCodes.EmailLabsDeliveryError;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}