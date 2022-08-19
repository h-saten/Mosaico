using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Storage.Base;
using Serilog;

namespace Mosaico.Application.DocumentManagement.Commands.DAO.CompanyDocumentFile
{
    public class CompanyDocumentFileCommandHandler : IRequestHandler<CompanyDocumentFileCommand, string>
    {
        private readonly IStorageClient _storageClient;
        private readonly ILogger _logger;

        public CompanyDocumentFileCommandHandler(IStorageClient storageClient, ILogger logger = null)
        {
            _storageClient = storageClient;
            _logger = logger;
        }
        
        public async Task<string> Handle(CompanyDocumentFileCommand request, CancellationToken cancellationToken)
        {
            var container = Constants.Containers.CompanyDocumentFileContainer;
            _logger?.Verbose($"Attempting to store {request.FileName} in container {container}");

            var fileId = await _storageClient.CreateAsync(new StorageObject
            {
                FileName = request.FileName,
                Container = container,
                Content = request.Content
            });

            _logger?.Verbose($"FIle {fileId} stored successfully");

            _logger?.Verbose($"Prepare to Profile URL");
            var profileUrl = await _storageClient.GetFileURLAsync(fileId, Constants.Containers.CompanyDocumentFileContainer);
            _logger?.Verbose($"Profile URL is Prepared {profileUrl}");

            return profileUrl;
        }
    }
}
