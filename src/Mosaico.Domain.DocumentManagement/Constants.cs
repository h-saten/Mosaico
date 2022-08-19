namespace Mosaico.Domain.DocumentManagement
{
    public static class Constants
    {
        public const string Schema = "doc";
        public static class Tables
        {
            public const string Documents = "Documents";
            public const string DocumentContents = "DocumentContents";
        }
        public static class ErrorCodes
        {
            public const string DocumentNotFound = "DOCUMENT_NOT_FOUND";
        }
    }
}
