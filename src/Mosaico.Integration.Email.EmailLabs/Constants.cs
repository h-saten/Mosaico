namespace Mosaico.Integration.Email.EmailLabs
{
    public static class Constants
    {
        public static class Resources
        {
            public const string NewEmail = "new_sendmail";
        }

        public static class ErrorCodes
        {
            public const string InvalidEmailPayload = "INVALID_EMAIL_PAYLOAD";
            public const string EmailLabsDeliveryError = "EMAIL_LABS_DELIVERY_ERROR";
            public const string EmailValidationError = "EMAIL_VALIDATION_ERROR";
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