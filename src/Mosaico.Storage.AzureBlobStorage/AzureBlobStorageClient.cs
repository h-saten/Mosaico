using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FluentValidation;
using Mosaico.Base;
using Mosaico.Storage.AzureBlobStorage.Configurations;
using Mosaico.Storage.AzureBlobStorage.Exceptions;
using Mosaico.Storage.AzureBlobStorage.Extensions;
using Mosaico.Storage.Base;
using Serilog;
using FileNotFoundException = Mosaico.Storage.AzureBlobStorage.Exceptions.FileNotFoundException;

namespace Mosaico.Storage.AzureBlobStorage
{
    //TODO: make container access type configurable
    public class AzureBlobStorageClient : IStorageClient
    {
        private readonly AzureBlobStorageConfiguration _configuration;
        private readonly IValidator<AzureBlobStorageConfiguration> _configValidator;
        private readonly ILogger _logger;
        
        public AzureBlobStorageClient(AzureBlobStorageConfiguration configuration, ILogger logger = null, IValidator<AzureBlobStorageConfiguration> configValidator = null)
        {
            _configuration = configuration;
            _logger = logger;
            _configValidator = configValidator;
        }

        protected virtual async Task<BlobContainerClient> GetClientAsync(string containerName, PublicAccessType accessType = PublicAccessType.None, CancellationToken? token = null)
        {
            token ??= CancellationToken.None;
            if (_configValidator != null)
            {
                var result = await _configValidator.ValidateAsync(_configuration, token.Value);
                if (!result.IsValid)
                {
                    var error = result.Errors.FirstOrDefault()?.ErrorMessage ?? "Blob Storage is misconfigured";
                    _logger?.Error(error);
                    throw new ValidationException(error);
                }
            }
            var service = new BlobServiceClient(_configuration.ConnectionString);
            var container = service.GetBlobContainerClient(containerName);
            _logger?.Verbose($"Trying to get or create {containerName} container");
            await container.CreateIfNotExistsAsync(accessType, null, token.Value);
            return container;
        }

        public async Task<StorageObject> GetObjectAsync(string id, string container)
        {
            _logger?.Verbose($"Trying to get an object {id} from {container} container");
            var client = await GetClientAsync(container, PublicAccessType.BlobContainer);
            var blob = client.GetBlobClient(id);
            var exists = await blob.ExistsAsync();
            if (!exists.Value)
            {
                throw new FileNotFoundException(id);
            }

            var downloadResponse = await blob.DownloadContentAsync();
            var content = downloadResponse.Value.Content.ToArray();
            _logger?.Verbose($"Downloaded content of {content.Length} bytes");
            var blobProperties = await blob.GetPropertiesAsync();
            return new StorageObject
            {
                FileId = id,
                Container = container,
                Content = content,
                Size = content.Length,
                FileName = blobProperties.GetMetadataValue(Constants.FileNameProperty),
                MimeType = MimeTypes.GetMimeType(blobProperties.GetMetadataValue(Constants.FileNameProperty))
            };
        }

        public Task<PaginatedResult<StorageItem>> GetObjectsAsync(string container, int take = 10, int skip = 0)
        {
            // var client = await GetClientAsync(container, PublicAccessType.BlobContainer);
            // var properties = await client.GetPropertiesAsync();
            // var response = new PaginatedResult<StorageItem>
            // {
            //     Total = 0,
            //     Entities = new List<StorageItem>()
            // };
            // if (int.TryParse(properties.GetMetadataValue(Constants.BlobCountProperty), out var countPropValue))
            // {
            //     response.Total = countPropValue;
            // }
            //
            // var paginator = client.GetBlobs(BlobTraits.All, BlobStates.All);
            //
            throw new NotImplementedException();
        }

        public async Task<StorageItem> CreateAsync(string fileName, byte[] content, string container, bool generateFileName = true, bool overwrite = true)
        {
            _logger?.Verbose($"Attempting to create {fileName} in {container}");
            var client = await GetClientAsync(container, PublicAccessType.BlobContainer);
            var containerProperties = await client.GetPropertiesAsync();
            var currentBlobCount = 0;
            if (int.TryParse(containerProperties.GetMetadataValue(Constants.BlobCountProperty), out var countPropValue))
            {
                currentBlobCount = countPropValue;
            }

            var id = GenerateFileId(fileName, generateFileName);
            
            _logger?.Verbose($"New file ID is {id}");
            var blob = client.GetBlobClient(id);
            if (!overwrite)
            {
                var blobExists = await blob.ExistsAsync();
                if (blobExists.Value)
                {
                    throw new FileDuplicateException(id);
                }
            }

            var fileContentType = MimeTypes.GetMimeType(fileName);
            await using (var stream = new MemoryStream(content))
            {
                await blob.UploadAsync(stream, new BlobHttpHeaders { ContentType = fileContentType }, conditions: overwrite ? null : new BlobRequestConditions { IfNoneMatch = new ETag("*") });
            }
            _logger?.Verbose($"File uploaded. Transfered {content.Length} bytes");
            await blob.SetMetadataAsync(new Dictionary<string, string>
            {
                {Constants.FileNameProperty, fileName}
            });
            currentBlobCount++;
            await client.SetMetadataAsync(new Dictionary<string, string>
            {
                {Constants.BlobCountProperty, currentBlobCount.ToString()}
            });
            return new StorageItem
            {
                FileId = id,
                Container = container,
                MimeType = fileContentType,
                Size = content.Length,
                FileName = fileName
            };
        }

        public async Task<string> CreateAsync(StorageObject obj, bool generateFileName = true, bool overwrite = true)
        {
            var item = await CreateAsync(obj.FileName, obj.Content, obj.Container, generateFileName, overwrite);
            return item.FileId;
        }

        public async Task DeleteAsync(string id, string container)
        {
            _logger?.Verbose($"Attempting to delete {id } from {container}");
            var client = await GetClientAsync(container, PublicAccessType.BlobContainer);
            var containerProperties = await client.GetPropertiesAsync();
            var currentBlobCount = 0;
            if (int.TryParse(containerProperties.GetMetadataValue(Constants.BlobCountProperty), out var countPropValue))
            {
                currentBlobCount = countPropValue;
            }
            
            var blob = client.GetBlobClient(id);
            var blobExists = await blob.ExistsAsync();
            if (!blobExists.Value)
            {
                throw new FileNotFoundException(id);
            }

            await blob.DeleteIfExistsAsync();
            _logger?.Verbose($"Blob deleted");
            currentBlobCount--;
            await client.SetMetadataAsync(new Dictionary<string, string>
            {
                {Constants.BlobCountProperty, currentBlobCount.ToString()}
            });
        }

        public Task DeleteAsync(StorageObject obj)
        {
            return DeleteAsync(obj.FileId, obj.Container);
        }

        public async Task<string> GetFileURLAsync(string id, string container)
        {
            _logger?.Verbose($"Attempting to get URL of {id} from {container}");
            var client = await GetClientAsync(container, PublicAccessType.BlobContainer);
            var blob = client.GetBlobClient(id);
            var exists = await blob.ExistsAsync();
            if (!exists.Value)
            {
                throw new FileNotFoundException(id);
            }
            return Flurl.Url.Combine(_configuration.EndpointUrl, container, id);
        }

        private string GenerateFileId(string fileName, bool generateFileName)
        {
            var folder = string.Empty;
            if (fileName.Contains('/'))
            {
                var parts = fileName.Split('/', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries).ToList();
                fileName = parts.Last();
                if (parts.Count > 1)
                {
                    parts.Remove(parts.Last());
                    folder = string.Join('/', parts);
                }
            }

            if (string.IsNullOrWhiteSpace(folder))
            {
                return generateFileName ? $"{Guid.NewGuid()}{Path.GetExtension(fileName)}" : $"{fileName}";
            }
            
            return generateFileName ? $"{folder}/{Guid.NewGuid()}{Path.GetExtension(fileName)}" : $"{folder}/{fileName}";
        }
    }
}