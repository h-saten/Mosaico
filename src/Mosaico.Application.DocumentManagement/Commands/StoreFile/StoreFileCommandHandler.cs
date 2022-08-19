using MediatR;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.DocumentManagement;
using Mosaico.Storage.Base;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.Commands.StoreFile
{
    //TODO: make restricted
    public class StoreFileCommandHandler : IRequestHandler<StoreFileCommand, string>
    {
        private readonly IDocumentDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IStorageClient _storageClient;
        private readonly ILogger _logger;

        public StoreFileCommandHandler(IDocumentDbContext context, IEventFactory eventFactory, IEventPublisher eventPublisher, IStorageClient storageClient, ILogger logger = null)
        {
            _context = context;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _storageClient = storageClient;
            _logger = logger;
        }
        public async Task<string> Handle(StoreFileCommand request, CancellationToken cancellationToken)
        {
            var container = Constants.Containers.DefaultDocumentContainer;
            _logger?.Verbose($"Attempting to store {request.FileName} in container {container}");

            var fileId = await _storageClient.CreateAsync(new StorageObject
            {
                FileName = request.FileName,
                Container = container,
                Content = request.Content
            });

            _logger?.Verbose($"FIle {fileId} stored successfully");

            _logger?.Verbose($"Attempting to send events");
            await PublishFileStoredEvent(fileId);
            _logger?.Verbose($"Events were sent");

            return fileId;
        }

        private async Task PublishFileStoredEvent(string fileId)
        {
            var eventPayload = new FileStoredEvent(fileId);
            var @event = _eventFactory.CreateEvent(Events.DocumentManagement.Constants.EventPaths.Documents, eventPayload);
            await _eventPublisher.PublishAsync(@event.Source, @event);
        }
    }
}
