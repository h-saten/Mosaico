using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertProjectDocuments
{
    public class UpsertProjectDocumentsCommandHandler : IRequestHandler<UpsertProjectDocumentsCommand, Guid>
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _projectDbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;

        public UpsertProjectDocumentsCommandHandler(ILogger logger, IProjectDbContext projectDbContext, IEventPublisher eventPublisher, IEventFactory eventFactory)
        {
            _logger = logger;
            _projectDbContext = projectDbContext;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
        }

        public async Task<Guid> Handle(UpsertProjectDocumentsCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to create project document");
            
            var project = await _projectDbContext.Projects
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
            
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }
            
            _logger?.Verbose($"Project {request.ProjectId} was found");
            var documentType = await _projectDbContext.DocumentTypes.FirstOrDefaultAsync(d => d.Key == request.Type, cancellationToken);
            if (documentType == null)
            {
                throw new DocumentTypeNotFoundException(request.Type);
            }
            
            var document = project.Documents.FirstOrDefault(d => d.TypeId == documentType.Id && d.Language == request.Language);
            if (document == null)
            {
                document = new Document
                {
                    Type = documentType,
                    TypeId = documentType.Id,
                    ProjectId = project.Id,
                    Project = project,
                    Content = request.Content,
                    Language = request.Language
                };
                _projectDbContext.Documents.Add(document);
            }
            else
            {
                document.Content = request.Content;
            }

            await _projectDbContext.SaveChangesAsync(cancellationToken);

            await PublishEvents(document.Id);
            return document.Id;
        }

        private async Task PublishEvents(Guid documentId)
        {
            var e = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, new ProjectDocumentUpdated(documentId));
            await _eventPublisher.PublishAsync(e);
        }
    }
}