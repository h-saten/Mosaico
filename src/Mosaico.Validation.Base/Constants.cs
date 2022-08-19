namespace Mosaico.Validation.Base
{
    public static class Constants
    {
        public static class ValidatorTypes
        {
            public const string OnCreate = "ON_CREATE";
            public const string OnUpdate = "ON_UPDATE";
            public const string OnDelete = "ON_DELETE";
        }

        public static class ErrorCodes
        {
            public const string UnexpectedValidationError = "UNEXPECTED_VALIDATION_ERROR";
        }
    }
}