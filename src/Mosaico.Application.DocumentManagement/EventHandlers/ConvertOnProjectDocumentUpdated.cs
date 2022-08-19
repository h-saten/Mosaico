using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.DocumentExport.Base;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Domain.DocumentManagement.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Mosaico.Storage.Base;

namespace Mosaico.Application.DocumentManagement.EventHandlers
{
    [EventInfo(nameof(ConvertOnProjectDocumentUpdated), "projects:api")]
    [EventTypeFilter(typeof(ProjectDocumentUpdated))]
    public class ConvertOnProjectDocumentUpdated : EventHandlerBase
    {
        private readonly IProjectManagementClient _managementClient;
        private readonly IDocumentExportClient _documentExport;
        private readonly IStorageClient _storageClient;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IDocumentDbContext _documentDbContext;

        public ConvertOnProjectDocumentUpdated(IProjectManagementClient managementClient, IDocumentExportClient documentExport, IStorageClient storageClient, IDocumentDbContext documentDbContext, IEventFactory eventFactory, IEventPublisher eventPublisher)
        {
            _managementClient = managementClient;
            _documentExport = documentExport;
            _storageClient = storageClient;
            _documentDbContext = documentDbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var projectDocumentEvent = @event?.GetData<ProjectDocumentUpdated>();
            if (projectDocumentEvent != null)
            {
                var document = await _managementClient.GetProjectDocumentAsync(projectDocumentEvent.ProjectDocumentId);
                if (document != null)
                {
                    var pdfBytes = await _documentExport.ExportAsync(document.Content);
                    if (pdfBytes != null && pdfBytes.Length > 0)
                    {
                        var fileId = await _storageClient.CreateAsync(new StorageObject
                        {
                            Container = "ci-documents",
                            Content = pdfBytes,
                            Size = pdfBytes.Length,
                            FileName = "test.pdf",
                            MimeType = "application/pdf"
                        });
                        var url = await _storageClient.GetFileURLAsync(fileId, "ci-documents");
                        _documentDbContext.ProjectDocuments.Add(new ProjectDocument
                        {
                            ProjectId = document.ProjectId,
                            Contents = new List<DocumentContent>
                            {
                                new DocumentContent
                                {
                                    Language = document.Language,
                                    FileId = fileId,
                                    DocumentAddress = url
                                }
                            }
                        });
                        var newEvent = _eventFactory.CreateEvent(
                            Events.ProjectManagement.Constants.EventPaths.Projects,
                            new ProjectDocumentConverted(document.Id, url));
                        await _eventPublisher.PublishAsync(newEvent);
                        await _documentDbContext.SaveChangesAsync();

                    }
                }
            }
        }
    }
}