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

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadPageCover
{
    public class UploadPageCoverCommandHandler : IRequestHandler<UploadPageCoverCommand, string>
    {
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IStorageClient _storageClient;
        private readonly ILogger _logger;
        private readonly IDocumentDbContext _documentDbContext;
        private readonly IDocumentUploadService _documentUploadService;

        public UploadPageCoverCommandHandler(IEventFactory eventFactory, IEventPublisher eventPublisher, IStorageClient storageClient, IDocumentDbContext documentDbContext, ILogger logger, IDocumentUploadService documentUploadService)
        {
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _storageClient = storageClient;
            _documentDbContext = documentDbContext;
            _logger = logger;
            _documentUploadService = documentUploadService;
        }

        public async Task<string> Handle(UploadPageCoverCommand request, CancellationToken cancellationToken)
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
                var cover = await _documentDbContext.PageCovers.Include(pl => pl.Contents)
                    .FirstOrDefaultAsync(p => p.PageId == request.PageId, cancellationToken);
                if (cover == null)
                {
                    _logger?.Verbose($"Page cover not found previously. Added new one");
                    await _documentUploadService.AddNewDocumentAsync<PageCover, UploadPageCoverCommand>(request, fileId, documentUrl, request.Language);
                }
                else
                {
                    var content = cover.Contents?.FirstOrDefault(c => c.Language == request.Language);
                    if (content != null)
                    {
                        await _storageClient.DeleteAsync(content.FileId, container);
                    }
                    _logger?.Verbose($"Page cover existed before. Updating it.");
                    await _documentUploadService.UpdateExistingDocumentAsync(cover, fileId, documentUrl, container, request.Language);
                }
                await _documentDbContext.SaveChangesAsync(cancellationToken);
                _logger?.Verbose($"Database record was successfully created");
                _logger?.Verbose($"Attempting to send events");
                await PublishEventsAsync(request.PageId, documentUrl, request.Language);
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

        private async Task PublishEventsAsync(Guid pageId, string logoUrl, string language)
        {
            var payload = new PageCoverUploaded(pageId, language, logoUrl);
            var @event = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, payload);
            await _eventPublisher.PublishAsync(@event);
        }
    }
}