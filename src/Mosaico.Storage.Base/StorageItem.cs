namespace Mosaico.Storage.Base
{
    public class StorageItem
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public long Size { get; set; }
        public string Container { get; set; }
    }
}