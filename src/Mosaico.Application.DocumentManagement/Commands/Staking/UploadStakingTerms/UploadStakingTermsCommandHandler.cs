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
using Mosaico.Events.Wallet;
using Mosaico.Storage.Base;
using Serilog;

namespace Mosaico.Application.DocumentManagement.Commands.Staking.UploadStakingTerms
{
    public class UploadStakingTermsCommandHandler : IRequestHandler<UploadStakingTermsCommand, string>
    {
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IStorageClient _storageClient;
        private readonly ILogger _logger;
        private readonly IDocumentDbContext _documentDbContext;
        private readonly IDocumentUploadService _documentUpload;

        public UploadStakingTermsCommandHandler(ILogger logger, IStorageClient storageClient, IEventPublisher eventPublisher, IEventFactory eventFactory, IDocumentDbContext documentDbContext, IDocumentUploadService documentUpload)
        {
            _logger = logger;
            _storageClient = storageClient;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _documentDbContext = documentDbContext;
            _documentUpload = documentUpload;
        }

        public async Task<string> Handle(UploadStakingTermsCommand request, CancellationToken cancellationToken)
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
                var terms = await _documentDbContext.StakingTermsDocuments.Include(pl => pl.Contents)
                    .FirstOrDefaultAsync(p => p.StakingPairId == request.StakingPairId, cancellationToken);
                if (terms == null)
                {
                    _logger?.Verbose($"Token logo was not found previously. Added new one");
                    await _documentUpload.AddNewDocumentAsync<StakingTermsDocument, UploadStakingTermsCommand>(request, fileId, documentUrl);
                }
                else
                {
                    var content = terms.Contents?.FirstOrDefault(c => c.Language == request.Language);
                    if (content != null)
                    {
                        await _storageClient.DeleteAsync(content.FileId, container);
                    }

                    _logger?.Verbose($"Token logo existed before. Updating it.");
                    await _documentUpload.UpdateExistingDocumentAsync(terms, fileId, documentUrl, container);
                }
                _logger?.Verbose($"Database record was successfully created");
                _logger?.Verbose($"Attempting to send events");
                await PublishEventsAsync(request.StakingPairId, documentUrl, request.Language);
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
        
        private async Task PublishEventsAsync(Guid stakePairId, string logoUrl, string language)
        {
            var payload = new StakingTermsUploaded(stakePairId, language, logoUrl);
            var @event = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets, payload);
            await _eventPublisher.PublishAsync(@event);
        }
    }
}