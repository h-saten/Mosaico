using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.DocumentManagement.Exceptions;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Domain.DocumentManagement.Entities;
using Mosaico.Domain.DocumentManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.DocumentManagement;
using Serilog;

namespace Mosaico.Application.DocumentManagement.Commands.DAO.DeleteCompanyDocument
{
    public class DeleteCompanyDocumentCommandHandler : IRequestHandler<DeleteCompanyDocumentCommand, Unit>
    {
        private readonly IDocumentDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;

        public DeleteCompanyDocumentCommandHandler(IDocumentDbContext context, IEventFactory eventFactory, IEventPublisher eventPublisher, ILogger logger = null)
        {
            _context = context;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }
        public async Task<Unit> Handle(DeleteCompanyDocumentCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"Attempting to remove a company document {request.Id}");

                    var document = await _context.CompanyDocuments
                        .FirstOrDefaultAsync(dc => dc.Id.Equals(request.Id) && dc.CompanyId == request.CompanyId, cancellationToken: cancellationToken);

                    if (document == null)
                        throw new DocumentNotFoundException(request.Id.ToString());

                    if (document.IsMandatory)
                        throw new DocumentIsMandatoryException(request.Id.ToString());

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
