using Azure;
using Azure.Storage.Blobs.Models;

namespace Mosaico.Storage.AzureBlobStorage.Extensions
{
    public static class BlobStorageExtensions
    {
        public static string GetMetadataValue(this BlobProperties properties, string name)
        {
            return properties.Metadata.TryGetValue(name, out var value) ? value : string.Empty;
        }

        public static string GetMetadataValue(this Response<BlobProperties> properties, string name)
        {
            return properties.Value.GetMetadataValue(name);
        }
        
        public static string GetMetadataValue(this BlobContainerProperties properties, string name)
        {
            return properties.Metadata.TryGetValue(name, out var value) ? value : string.Empty;
        }

        public static string GetMetadataValue(this Response<BlobContainerProperties> properties, string name)
        {
            return properties.Value.GetMetadataValue(name);
        }
    }
}