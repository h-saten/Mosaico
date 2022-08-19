using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.DocumentManagement.DTOs;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Domain.DocumentManagement.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.Storage.Base;
using Serilog;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadArticlePhoto
{
    public class UploadArticlePhotoCommandHandler : IRequestHandler<UploadArticlePhotoCommand, UpdateArticleDTO>
    {
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IStorageClient _storageClient;
        private readonly ILogger _logger;
        private readonly IDocumentDbContext _documentDbContext;

        public UploadArticlePhotoCommandHandler(IEventFactory eventFactory, IEventPublisher eventPublisher, IStorageClient storageClient, IDocumentDbContext documentDbContext, ILogger logger)
        {
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _storageClient = storageClient;
            _documentDbContext = documentDbContext;
            _logger = logger;
        }

        public async Task<UpdateArticleDTO> Handle(UploadArticlePhotoCommand request, CancellationToken cancellationToken)
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
            var documentUrl = "";
            try
            {
                _logger?.Verbose($"Attempting to add new document entry to database");
                documentUrl = await _storageClient.GetFileURLAsync(fileId, container);
                var photo = await _documentDbContext.ArticlePhotos.Include(pl => pl.Contents)
                    .FirstOrDefaultAsync(p => p.ArticleId == request.ArticleId, cancellationToken);
                var photoId = request.ArticleId;
                if (photo == null)
                {
                    _logger?.Verbose($"Article photo not found previously. Added new one");
                   photoId = await AddNewDocumentAsync(request, fileId, documentUrl);
                }
                else
                {
                    _logger?.Verbose($"Article photo existed before. Updating it.");
                    await UpdateExistingDocumentAsync(photo, fileId, documentUrl, container, request.Language);
                }
                await _documentDbContext.SaveChangesAsync(cancellationToken);
                _logger?.Verbose($"Database record was successfully created");
                return new UpdateArticleDTO { ArticleFileId = photoId.ToString(), FileURL = documentUrl };
            }
            catch (Exception)
            {
                if (!string.IsNullOrWhiteSpace(fileId))
                {
                    await _storageClient.DeleteAsync(fileId, container);
                }

                throw;
            }
        }
        
        private Task<Guid> AddNewDocumentAsync(UploadArticlePhotoCommand request, string fileId, string documentUrl)
        {
            var photo = new ArticlePhoto
            {
                Title = request.FileName,
                ArticleId = Guid.NewGuid()
            };
            _documentDbContext.ArticlePhotos.Add(photo);
            photo.Contents.Add(new DocumentContent
            {
                Language = request.Language,
                FileId = fileId,
                DocumentAddress = documentUrl,
                DocumentId = photo.Id,
                Document = photo
            });
            return Task.FromResult(photo.Id);
        }

        private async Task UpdateExistingDocumentAsync(ArticlePhoto photo, string fileId, string documentUrl, string container, string language)
        {
            var content = photo.Contents.FirstOrDefault(c => c.Language == language);
            if (content == null)
            {
                content = new DocumentContent
                {
                    Language = language,
                    FileId = fileId,
                    DocumentAddress = documentUrl
                };
                photo.Contents.Add(content);
            }
            else
            {
                try
                {
                    _logger?.Verbose($"Attempting to remove old article photo {content.FileId}");
                    await _storageClient.DeleteAsync(content.FileId, container);
                    _logger?.Verbose($"Old article photo was successfully removed");
                }
                catch (Exception ex)
                {
                    _logger?.Error(
                        $"Failed to delete existing article cover from {container} - {content.FileId}: {ex.Message}/{ex.StackTrace}");
                }

                content.FileId = fileId;
                content.DocumentAddress = documentUrl;
            }
        }

        private async Task PublishEventsAsync(Guid pageId, string logoUrl)
        {
            var payload = new ArticlePhotoUploaded(pageId, logoUrl);
            var @event = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, payload);
            await _eventPublisher.PublishAsync(@event);
        }
    }
}