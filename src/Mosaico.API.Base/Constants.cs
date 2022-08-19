namespace Mosaico.API.Base
{
    public static class Constants
    {
        public static class ErrorCodes
        {
            public const string InvalidParameters = "INVALID_PARAMETERS";
            public const string UnsupportedModule = "UNSUPPORTED_MODULE";
            public const string FileSizeLimitExceeded = "FILE_SIZE_LIMIT_EXCEEDED";
            public const string NotPermittedFileExtension = "NOT_PERMITTED_FILE_EXTENSION";
        }
        
        public static class Permissions
        {
            public const string AppServicesInternalActions = "AppServicesInternalActions";
        }
    }
}