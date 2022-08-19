using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.DocumentManagement.Exceptions;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Domain.DocumentManagement.Entities;
using Mosaico.Domain.DocumentManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.DocumentManagement;
using Serilog;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.UpdateProjectDocument
{
    public class UpdateProjectDocumentCommandHandler : IRequestHandler<UpdateProjectDocumentCommand>
    {
        private readonly IDocumentDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;

        public UpdateProjectDocumentCommandHandler(IDocumentDbContext context, IMapper mapper, IEventFactory eventFactory, IEventPublisher eventPublisher, ILogger logger = null)
        {
            _context = context;
            _mapper = mapper;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }
        public async Task<Unit> Handle(UpdateProjectDocumentCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"Attempting to update a project's document");

                    var document = await _context.ProjectDocuments.FirstOrDefaultAsync(
                        p => p.ProjectId == request.ProjectId && p.Id == request.Id, cancellationToken);
                    
                    if (document == null)
                        throw new DocumentNotFoundException(request.Id.ToString());
                    _logger?.Verbose($"Project was found");

                    if(document.IsMandatory)
                        throw new DocumentIsMandatoryException(request.Id.ToString());

                    if (_context.ProjectDocuments.Any(pd => pd.ProjectId.Equals(document.ProjectId)  && !pd.Id.Equals(document.Id) && pd.Title == request.Title))
                        throw new DuplicateDocumentTitleException(request.Title);

                    _mapper.Map(request, document);

                    await _context.SaveChangesAsync(cancellationToken);
                    _logger?.Verbose($"Project's document with id {document.Id} was successfully updated");

                    _logger?.Verbose($"Attempting to send events");
                    await PublishDocumentUpdatedEventAsync(document);
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

        private async Task PublishDocumentUpdatedEventAsync(ProjectDocument document)
        {
            var eventPayload = new DocumentUpdatedEvent(document.Id, document.Title);
            var @event = _eventFactory.CreateEvent(Events.DocumentManagement.Constants.EventPaths.Documents, eventPayload);
            await _eventPublisher.PublishAsync(@event.Source, @event);
        }
    }
}
