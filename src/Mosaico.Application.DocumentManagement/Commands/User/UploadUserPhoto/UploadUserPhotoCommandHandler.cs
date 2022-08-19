using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Domain.DocumentManagement.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Mosaico.Storage.Base;
using Serilog;

namespace Mosaico.Application.DocumentManagement.Commands.User.UploadUserPhoto
{
    public class UploadUserPhotoCommandHandler : IRequestHandler<UploadUserPhotoCommand, string>
    {
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IStorageClient _storageClient;
        private readonly ILogger _logger;
        private readonly IDocumentDbContext _documentDbContext;

        public UploadUserPhotoCommandHandler(IDocumentDbContext documentDbContext, IStorageClient storageClient, IEventPublisher eventPublisher, IEventFactory eventFactory, ILogger logger = null)
        {
            _documentDbContext = documentDbContext;
            _storageClient = storageClient;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _logger = logger;
        }

        public async Task<string> Handle(UploadUserPhotoCommand request, CancellationToken cancellationToken)
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
                var userPhoto = await _documentDbContext.UserPhotos.Include(pl => pl.Contents).FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);
                if (userPhoto == null)
                {
                    _logger?.Verbose($"User photo was not found previously. Added new one");
                    await AddNewDocumentAsync(request, fileId, documentUrl);
                }
                else
                {
                    _logger?.Verbose($"User photo existed before. Updating it.");
                    await UpdateExistingDocumentAsync(userPhoto, fileId, documentUrl, container);
                }
                await _documentDbContext.SaveChangesAsync(cancellationToken);
                _logger?.Verbose($"Database record was successfully created");
                _logger?.Verbose($"Attempting to send events");
                await PublishEventsAsync(request.UserId.ToString(), documentUrl);
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
        
        private Task AddNewDocumentAsync(UploadUserPhotoCommand request, string fileId, string documentUrl)
        {
            var userPhoto = new UserPhoto
            {
                Title = request.FileName,
                UserId = request.UserId
            };
            _documentDbContext.UserPhotos.Add(userPhoto);
            userPhoto.Contents.Add(new DocumentContent
            {
                Language = Base.Constants.Languages.English,
                FileId = fileId,
                DocumentAddress = documentUrl,
                DocumentId = userPhoto.Id,
                Document = userPhoto
            });
            return Task.CompletedTask;
        }

        private async Task UpdateExistingDocumentAsync(UserPhoto userPhoto, string fileId, string documentUrl, string container)
        {
            var content = userPhoto.Contents.FirstOrDefault();
            if (content == null)
            {
                content = new DocumentContent
                {
                    Language = Base.Constants.Languages.English,
                    FileId = fileId,
                    DocumentAddress = documentUrl
                };
                userPhoto.Contents.Add(content);
            }
            else
            {
                try
                {
                    _logger?.Verbose($"Attempting to remove old photo {content.FileId}");
                    await _storageClient.DeleteAsync(content.FileId, container);
                    _logger?.Verbose($"Old photo was successfully removed");
                }
                catch (Exception ex)
                {
                    _logger?.Error(
                        $"Failed to delete existing user photo from {container} - {content.FileId}: {ex.Message}/{ex.StackTrace}");
                }

                content.FileId = fileId;
                content.DocumentAddress = documentUrl;
            }
        }

        private async Task PublishEventsAsync(string userId, string logoUrl)
        {
            var payload = new UserPhotoUploaded(userId, logoUrl);
            var @event = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, payload);
            await _eventPublisher.PublishAsync(@event);
        }
    }
}