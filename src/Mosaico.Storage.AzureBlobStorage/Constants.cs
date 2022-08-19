namespace Mosaico.Storage.AzureBlobStorage
{
    public static class Constants
    {
        public const string FileNameProperty = "fileName";
        public const string BlobCountProperty = "count";

        public static class ErrorCodes
        {
            public const string DuplicateFile = "FILE_EXISTS";
            public const string FileNotFound = "FILE_NOT_FOUND";
        }
    }
}