using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.DocumentManagement.Abstractions;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Domain.DocumentManagement.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.Storage.Base;
using Serilog;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadProjectLogo
{
    public class UploadProjectLogoCommandHandler : IRequestHandler<UploadProjectLogoCommand, string>
    {
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IStorageClient _storageClient;
        private readonly ILogger _logger;
        private readonly IDocumentDbContext _documentDbContext;
        private readonly IDocumentUploadService _documentUploadService;

        public UploadProjectLogoCommandHandler(IEventFactory eventFactory, IEventPublisher eventPublisher, IStorageClient storageClient, IDocumentDbContext documentDbContext, IDocumentUploadService documentUploadService, ILogger logger = null)
        {
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _storageClient = storageClient;
            _documentDbContext = documentDbContext;
            _documentUploadService = documentUploadService;
            _logger = logger;
        }

        public async Task<string> Handle(UploadProjectLogoCommand request, CancellationToken cancellationToken)
        {
            var container = Constants.Containers.DefaultDocumentContainer;
            _logger?.Verbose($"Attempting to store {request.FileName} in container {container}");
            var fileId = await _storageClient.CreateAsync(new StorageObject
            {
                FileName = request.FileName,
                Container = container,
                Content = request.Content
            });
            _logger?.Verbose($"File {fileId} stored successfully");
            try
            {
                _logger?.Verbose($"Attempting to add new document entry to database");
                var documentUrl = await _storageClient.GetFileURLAsync(fileId, container);
                var projectLogo = await _documentDbContext.ProjectLogos.Include(pl => pl.Contents).FirstOrDefaultAsync(p => p.ProjectId == request.ProjectId, cancellationToken);
                if (projectLogo == null) 
                {
                    _logger?.Verbose($"Project logo was not found previously. Added new one");
                    await _documentUploadService.AddNewDocumentAsync<ProjectLogo, UploadProjectLogoCommand>(request, fileId, documentUrl);
                }
                else
                {
                    var content = projectLogo.Contents?.FirstOrDefault(c => c.Language == request.Language);
                    if (content != null)
                    {
                        await _storageClient.DeleteAsync(content.FileId, container);
                    }
                    _logger?.Verbose($"Project logo existed before. Updating it.");
                    await _documentUploadService.UpdateExistingDocumentAsync(projectLogo, fileId, documentUrl, container);
                }
                await _documentDbContext.SaveChangesAsync(cancellationToken);
                _logger?.Verbose($"Database record was successfully created");
                _logger?.Verbose($"Attempting to send events");
                await PublishEventsAsync(request.ProjectId, documentUrl);
                _logger?.Verbose($"Events were sent");
            }
            catch (Exception)
            {
                if (!string.IsNullOrWhiteSpace(fileId))
                {
                    await _storageClient.DeleteAsync(fileId, container);
                }

                throw;
            }
            return fileId;
        }

        private Task AddNewDocumentAsync(UploadProjectLogoCommand request, string fileId, string documentUrl)
        {
            var projectLogo = new ProjectLogo
            {
                Title = request.FileName,
                ProjectId = request.ProjectId
            };
            _documentDbContext.ProjectLogos.Add(projectLogo);
            projectLogo.Contents.Add(new DocumentContent
            {
                Language = Base.Constants.Languages.English,
                FileId = fileId,
                DocumentAddress = documentUrl,
                DocumentId = projectLogo.Id,
                Document = projectLogo
            });
            return Task.CompletedTask;
        }

        private async Task UpdateExistingDocumentAsync(ProjectLogo projectLogo, string fileId, string documentUrl, string container)
        {
            var content = projectLogo.Contents.FirstOrDefault();
            if (content == null)
            {
                content = new DocumentContent
                {
                    Language = Base.Constants.Languages.English,
                    FileId = fileId,
                    DocumentAddress = documentUrl
                };
                projectLogo.Contents.Add(content);
            }
            else
            {
                try
                {
                    _logger?.Verbose($"Attempting to remove old logo {content.FileId}");
                    await _storageClient.DeleteAsync(content.FileId, container);
                    _logger?.Verbose($"Old logo was successfully removed");
                }
                catch (Exception ex)
                {
                    _logger?.Error(
                        $"Failed to delete existing project logo from {container} - {content.FileId}: {ex.Message}/{ex.StackTrace}");
                }

                content.FileId = fileId;
                content.DocumentAddress = documentUrl;
            }
        }

        private async Task PublishEventsAsync(Guid projectId, string logoUrl)
        {
            var payload = new ProjectLogoUploaded(projectId, logoUrl);
            var @event = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, payload);
            await _eventPublisher.PublishAsync(@event);
        }
    }
}