using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Integration.Sms.SmsLabs.Exceptions
{
    public class SmsLabsDeliveryException : ExceptionBase
    {
        public string Reason { get; set; }
        public SmsLabsDeliveryException(string reason) : base($"Failed to send an sms message via sms labs: {reason}")
        {
            Reason = reason;
        }

        public override string Code => Constants.ErrorCodes.InvalidSmsPayload;
        public override int StatusCode => StatusCodes.Status500InternalServerError;
    }
}