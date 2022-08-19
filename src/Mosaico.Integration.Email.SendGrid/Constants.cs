using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Integration.Email.SendGridEmail
{
    public static class Constants
    {
        public static class ErrorCodes
        {
            public const string InvalidEmailPayload = "INVALID_EMAIL_PAYLOAD";
            public const string EmailLabsDeliveryError = "EMAIL_SENDGRID_DELIVERY_ERROR";
            public const string EmailValidationError = "EMAIL_VALIDATION_ERROR";
        }
    }
}
