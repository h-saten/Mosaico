using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mosaico.Application.DocumentManagement.Exceptions;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Domain.DocumentManagement.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.DocumentManagement;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.DocumentManagement.Commands.DAO.CreateCompanyDocument
{
    public class CreateCompanyDocumentCommandHandler : IRequestHandler<CreateCompanyDocumentCommand, Guid>
    {
        private readonly IDocumentDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IBusinessManagementClient _companyManagementClient;
        private readonly ILogger _logger;

        public CreateCompanyDocumentCommandHandler(IDocumentDbContext context, IMapper mapper, IEventFactory eventFactory, IEventPublisher eventPublisher, IBusinessManagementClient companyManagementClient, ILogger logger = null)
        {
            _context = context;
            _mapper = mapper;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _companyManagementClient = companyManagementClient;
            _logger = logger;
        }
        public async Task<Guid> Handle(CreateCompanyDocumentCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"Attempting to create a document for company {request.CompanyId}");
                    var company = await _companyManagementClient.GetCompanyAsync(request.CompanyId, cancellationToken);
                    if (company == null)
                        throw new ProjectNotFoundException(request.CompanyId.ToString());

                    if (_context.CompanyDocuments.Any(pd => pd.CompanyId == request.CompanyId && pd.Title == request.Title))
                        throw new DuplicateDocumentTitleException(request.Title);

                    var document = _mapper.Map<CompanyDocument>(request);
                    _context.CompanyDocuments.Add(document);
                    await _context.SaveChangesAsync(cancellationToken);

                    _logger?.Verbose($"Document {document.Id} Created for company ${document.CompanyId}");


                    _logger?.Verbose($"Attempting to send events");
                    await PublishDocumentCreatedEvent(document);
                    _logger?.Verbose($"Events were sent");

                    await transaction.CommitAsync(cancellationToken);

                    return document.Id;
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private async Task PublishDocumentCreatedEvent(DocumentBase document)
        {
            var eventPayload = new DocumentCreatedEvent(document.Id, document.Title);
            var @event = _eventFactory.CreateEvent(Events.DocumentManagement.Constants.EventPaths.Documents, eventPayload);
            await _eventPublisher.PublishAsync(@event.Source, @event);
        }
    }
}
