using MediatR;
using Mosaico.Application.DocumentManagement.Exceptions;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Domain.DocumentManagement.Entities;
using Mosaico.Domain.DocumentManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.DocumentManagement;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Mosaico.Storage.Base;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mosaico.Application.DocumentManagement.Commands.RemoveDocumentContent
{
    public class RemoveProjectDocumentCommandHandler : IRequestHandler<RemoveProjectDocumentCommand, Unit>
    {
        private readonly IDocumentDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;
        private readonly IStorageClient _storageClient;

        public RemoveProjectDocumentCommandHandler(IDocumentDbContext context, IEventFactory eventFactory, IEventPublisher eventPublisher, IStorageClient storageClient, ILogger logger = null)
        {
            _context = context;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _storageClient = storageClient;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(RemoveProjectDocumentCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"Attempting to remove a content for document {request.DocumentId}");

                    var content = await _context.ProjectDocuments.Include(pd => pd.Contents)
                        .Where(p => p.ProjectId == request.ProjectId)
                        .SelectMany(pd => pd.Contents)
                        .FirstOrDefaultAsync(c => c.Id == request.DocumentId && c.Language == request.Language, cancellationToken);

                    if (content == null)
                        throw new DocumentLanguageNotAvailableException(request.Language);

                    await _storageClient.DeleteAsync(content.FileId, Constants.Containers.DefaultDocumentContainer);
                    
                    _context.DocumentContents.Remove(content);

                    await _context.SaveChangesAsync(cancellationToken);
                    _logger?.Verbose($"Document content {content.Id} in {content.Language} removed from document ${content.DocumentId}");

                    _logger?.Verbose($"Attempting to send events");
                    await PublishDocumentContentRemovedEvent(content);
                    _logger?.Verbose($"Events were sent");

                    await transaction.CommitAsync(cancellationToken);

                    return Unit.Value;
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task PublishDocumentContentRemovedEvent(DocumentContent documentContent)
        {
            var eventPayload = new DocumentContentRemovedEvent(documentContent.Id, documentContent.DocumentId);
            var @event = _eventFactory.CreateEvent(Events.DocumentManagement.Constants.EventPaths.Documents, eventPayload);
            await _eventPublisher.PublishAsync(@event.Source, @event);
        }
    }
}
