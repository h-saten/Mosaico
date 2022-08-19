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

namespace Mosaico.Application.DocumentManagement.Commands.DAO.UpdateCompanyDocument
{
    public class UpdateCompanyDocumentCommandHandler : IRequestHandler<UpdateCompanyDocumentCommand>
    {
        private readonly IDocumentDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger _logger;

        public UpdateCompanyDocumentCommandHandler(IDocumentDbContext context, IMapper mapper, IEventFactory eventFactory, IEventPublisher eventPublisher, ILogger logger = null)
        {
            _context = context;
            _mapper = mapper;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _logger = logger;
        }
        public async Task<Unit> Handle(UpdateCompanyDocumentCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"Attempting to update a company's document");

                    var document = await _context.CompanyDocuments.FirstOrDefaultAsync(d =>
                        d.Id == request.Id && d.CompanyId == request.CompanyId, cancellationToken: cancellationToken);
                    if (document == null)
                        throw new DocumentNotFoundException(request.Id.ToString());
                    _logger?.Verbose($"Company was found");

                    if(document.IsMandatory)
                        throw new DocumentIsMandatoryException(request.Id.ToString());

                    if (_context.CompanyDocuments.Any(pd => pd.CompanyId.Equals(document.CompanyId)  && !pd.Id.Equals(document.Id) && pd.Title == request.Title))
                        throw new DuplicateDocumentTitleException(request.Title);

                    _mapper.Map(request, document);

                    await _context.SaveChangesAsync(cancellationToken);
                    _logger?.Verbose($"Company's document with id {document.Id} was successfully updated");

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

        private async Task PublishDocumentUpdatedEventAsync(CompanyDocument document)
        {
            var eventPayload = new DocumentUpdatedEvent(document.Id, document.Title);
            var @event = _eventFactory.CreateEvent(Events.DocumentManagement.Constants.EventPaths.Documents, eventPayload);
            await _eventPublisher.PublishAsync(@event.Source, @event);
        }
    }
}
