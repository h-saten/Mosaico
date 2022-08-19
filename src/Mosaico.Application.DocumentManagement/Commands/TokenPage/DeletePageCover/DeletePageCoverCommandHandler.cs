using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.Storage.Base;
using Serilog;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.DeletePageCover
{
    public class DeletePageCoverCommandHandler : IRequestHandler<DeletePageCoverCommand>
    {
        private readonly IDocumentDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
        private readonly IStorageClient _storageClient;

        public DeletePageCoverCommandHandler(IDocumentDbContext context, IStorageClient storageClient, IEventFactory eventFactory, IEventPublisher eventPublisher, ILogger logger)
        {
            _context = context;
            _storageClient = storageClient;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeletePageCoverCommand request, CancellationToken cancellationToken)
        {
            var container = Constants.Containers.DefaultDocumentContainer;
            _logger?.Verbose($"Attempting to delete cover of page {request.PageId} in container {container}");
            var document =
                await _context.PageCovers.Include(d => d.Contents).FirstOrDefaultAsync(up => up.PageId == request.PageId,
                    cancellationToken);
            var content = document?.Contents.FirstOrDefault();
            if (document == null || content == null)
            {
                _logger?.Verbose($"Page cover document not found. Nothing to delete");
                return Unit.Value;
            }
            _logger?.Verbose($"Deleting file {content.FileId}");
            await _storageClient.DeleteAsync(content.FileId, container);
            _logger?.Verbose($"File {content.FileId} successfully deleted");
            _logger?.Verbose($"Removing records from database");
            _context.DocumentContents.Remove(content);
            _context.PageCovers.Remove(document);
            await _context.SaveChangesAsync(cancellationToken);
            _logger?.Verbose($"Records from database were successfully removed. Publishing events");
            await PublishEventsAsync(request.PageId, request.Language);
            return Unit.Value;
        }
        
        private async Task PublishEventsAsync(Guid pageId, string language)
        {
            var payload = new PageCoverUploaded(pageId, language, null);
            var @event = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, payload);
            await _eventPublisher.PublishAsync(@event);
        }
    }
}