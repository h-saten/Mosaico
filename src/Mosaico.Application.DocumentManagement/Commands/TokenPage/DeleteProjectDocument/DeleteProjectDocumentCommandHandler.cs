using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Domain.DocumentManagement.Entities;
using Mosaico.Domain.DocumentManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.DocumentManagement;
using Serilog;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.DeleteProjectDocument
{
    public class DeleteProjectDocumentCommandHandler : IRequestHandler<DeleteProjectDocumentCommand, Unit>
    {
        private readonly IDocumentDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;

        public DeleteProjectDocumentCommandHandler(IDocumentDbContext context, IEventFactory eventFactory, IEventPublisher eventPublisher, ILogger logger = null)
        {
            _context = context;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(DeleteProjectDocumentCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"Attempting to remove a project document {request.Id}");

                    var document = await _context.ProjectDocuments
                        .FirstOrDefaultAsync(dc => dc.ProjectId == request.ProjectId && dc.Id == request.Id, cancellationToken: cancellationToken);
                    
                    if (document == null)
                        throw new DocumentNotFoundException(request.Id.ToString());

                    _context.Documents.Remove(document);

                    await _context.SaveChangesAsync(cancellationToken);
                    _logger?.Verbose($"Document {document.Id} deleted");

                    _logger?.Verbose($"Attempting to send events");
                    await PublishDocumentDeletedEvent(document);
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

        private async Task PublishDocumentDeletedEvent(DocumentBase document)
        {
            var eventPayload = new DocumentDeletedEvent(document.Id);
            var @event = _eventFactory.CreateEvent(Events.DocumentManagement.Constants.EventPaths.Documents, eventPayload);
            await _eventPublisher.PublishAsync(@event.Source, @event);
        }
    }
}
