using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Queries.GetProjectDocuments;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.Storage.Base;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.UploadProjectDocuments
{
    public class UploadProjectDocumentsCommandHandler : IRequestHandler<UploadProjectDocumentsCommand, Guid>
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _projectDbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IStorageClient _storageClient;
        private readonly ICacheClient _cacheClient;
        public UploadProjectDocumentsCommandHandler(IStorageClient storageClient, ILogger logger, IProjectDbContext projectDbContext, IEventPublisher eventPublisher, IEventFactory eventFactory, ICacheClient cacheClient)
        {
            _logger = logger;
            _projectDbContext = projectDbContext;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _storageClient = storageClient;
            _cacheClient = cacheClient;
        }

        public async Task<Guid> Handle(UploadProjectDocumentsCommand request, CancellationToken cancellationToken)
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

            var fileId = await _storageClient.CreateAsync(new StorageObject
            {
                Container = "ci-documents",
                Content = request.Content,
                Size = request.Content.Length,
                FileName = $"{project.Slug}/{project.Slug}_{documentType.Key}_{request.Language}.pdf",
                MimeType = "application/pdf"
            }, false);
            var url = await _storageClient.GetFileURLAsync(fileId, "ci-documents");

            var existingDoc = project.Documents.FirstOrDefault(t => t.TypeId == documentType.Id && t.ProjectId == project.Id && t.Language == request.Language);
            if (existingDoc != null)
            {
                _projectDbContext.Documents.Remove(existingDoc);
            }
            var document = new Document
             {
                 Type = documentType,
                 TypeId = documentType.Id,
                 ProjectId = project.Id,
                 Project = project,
                 Content = "",
                 Language = request.Language,
                 Url = url
             };
            _projectDbContext.Documents.Add(document);
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            await _cacheClient.CleanAsync($"{nameof(GetProjectDocumentsQuery)}_{project.Id}_*", cancellationToken);
            await PublishEvents(document.Id, url);
            return document.Id;
        }

        private async Task PublishEvents(Guid documentId, string url)
        {
            var e = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, new ProjectDocumentUploaded(documentId,url));
            await _eventPublisher.PublishAsync(e);
        }
    }
}