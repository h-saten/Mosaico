namespace Mosaico.Integration.Sms.SmsLabs
{
    public static class Constants
    {
        public static class Resources
        {
            public const string SendSms = "sendSms";
        }

        public static class ErrorCodes
        {
            public const string InvalidSmsPayload = "INVALID_SMS_PAYLOAD";
            public const string SmsLabsDeliveryError = "SMS_LABS_DELIVERY_ERROR";
            public const string SmsValidationError = "SMS_VALIDATION_ERROR";
        }

        public static class Parameters
        {
            public const string ApplicationKey = "AppKey";
            public const string SMTPAccount = "smtp_account";
            public const string FromEmail = "from";
            public const string DisplayName = "from_name";
            public const string Subject = "subject";
            public const string Html = "html";
        }
    }
}