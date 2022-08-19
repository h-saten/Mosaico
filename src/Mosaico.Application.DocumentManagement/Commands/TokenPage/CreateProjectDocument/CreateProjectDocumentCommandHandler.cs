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
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.CreateProjectDocument
{
    public class CreateProjectDocumentCommandHandler : IRequestHandler<CreateProjectDocumentCommand, Guid>
    {
        private readonly IDocumentDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly ILogger _logger;

        public CreateProjectDocumentCommandHandler(IDocumentDbContext context, IMapper mapper, IEventFactory eventFactory, IEventPublisher eventPublisher, IProjectManagementClient projectManagementClient, ILogger logger = null)
        {
            _context = context;
            _mapper = mapper;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _projectManagementClient = projectManagementClient;
            _logger = logger;
        }
        public async Task<Guid> Handle(CreateProjectDocumentCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _context.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"Attempting to create a document for project {request.ProjectId}");
                    var project = await _projectManagementClient.GetProjectAsync(request.ProjectId, cancellationToken);
                    if (project == null)
                        throw new ProjectNotFoundException(request.ProjectId.ToString());

                    if (_context.ProjectDocuments.Any(pd => pd.ProjectId == request.ProjectId && pd.Title == request.Title))
                        throw new DuplicateDocumentTitleException(request.Title);

                    var document = _mapper.Map<ProjectDocument>(request);
                    _context.ProjectDocuments.Add(document);
                    await _context.SaveChangesAsync(cancellationToken);

                    _logger?.Verbose($"Document {document.Id} Created for project ${document.ProjectId}");


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
