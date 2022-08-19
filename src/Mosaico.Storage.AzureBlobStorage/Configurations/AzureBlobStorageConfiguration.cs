namespace Mosaico.Storage.AzureBlobStorage.Configurations
{
    public class AzureBlobStorageConfiguration
    {
        public const string SectionName = "AzureBlobStorage";
        public string ConnectionString { get; set; }
        public string EndpointUrl { get; set; }
    }
}