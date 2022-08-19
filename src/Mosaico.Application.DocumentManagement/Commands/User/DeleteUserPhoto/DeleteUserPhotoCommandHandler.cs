using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.DocumentManagement;
using Mosaico.Storage.Base;
using Serilog;

namespace Mosaico.Application.DocumentManagement.Commands.User.DeleteUserPhoto
{
    public class DeleteUserPhotoCommandHandler : IRequestHandler<DeleteUserPhotoCommand>
    {
        private readonly ILogger _logger;
        private readonly IStorageClient _storageClient;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IDocumentDbContext _documentDbContext;

        public DeleteUserPhotoCommandHandler(IStorageClient storageClient, IEventFactory eventFactory, IEventPublisher eventPublisher, IDocumentDbContext documentDbContext, ILogger logger = null)
        {
            _logger = logger;
            _storageClient = storageClient;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _documentDbContext = documentDbContext;
        }

        public async Task<Unit> Handle(DeleteUserPhotoCommand request, CancellationToken cancellationToken)
        {
            var container = Constants.Containers.DefaultDocumentContainer;
            _logger?.Verbose($"Attempting to delete photo of user {request.UserId} in container {container}");
            var document =
                await _documentDbContext.UserPhotos.Include(d => d.Contents).FirstOrDefaultAsync(up => up.UserId == request.UserId,
                    cancellationToken);
            var content = document?.Contents.FirstOrDefault();
            if (document == null || content == null)
            {
                _logger?.Verbose($"User photo document not found. Nothing to delete");
                return Unit.Value;
            }
            _logger?.Verbose($"Deleting file {content.FileId}");
            await _storageClient.DeleteAsync(content.FileId, container);
            _logger?.Verbose($"File {content.FileId} successfully deleted");
            _logger?.Verbose($"Removing records from database");
            _documentDbContext.DocumentContents.Remove(content);
            _documentDbContext.UserPhotos.Remove(document);
            await _documentDbContext.SaveChangesAsync(cancellationToken);
            _logger?.Verbose($"Records from database were successfully removed. Publishing events");
            await PublishEventAsync(request.UserId);
            return Unit.Value;
        }

        private async Task PublishEventAsync(Guid userId)
        {
            var e = _eventFactory.CreateEvent(Events.DocumentManagement.Constants.EventPaths.Documents,
                new UserPhotoDeletedEvent(userId));
            await _eventPublisher.PublishAsync(e);
        }
    }
}