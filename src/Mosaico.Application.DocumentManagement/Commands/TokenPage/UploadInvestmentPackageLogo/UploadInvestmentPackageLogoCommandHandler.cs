using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Domain.DocumentManagement.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.Storage.Base;
using Serilog;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadInvestmentPackageLogo
{
    public class UploadInvestmentPackageLogoCommandHandler : IRequestHandler<UploadInvestmentPackageLogoCommand, string>
    {
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IStorageClient _storageClient;
        private readonly ILogger _logger;
        private readonly IDocumentDbContext _documentDbContext;

        public UploadInvestmentPackageLogoCommandHandler(IEventFactory eventFactory, IEventPublisher eventPublisher, IStorageClient storageClient, IDocumentDbContext documentDbContext, ILogger logger = null)
        {
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _storageClient = storageClient;
            _documentDbContext = documentDbContext;
            _logger = logger;
        }

        public async Task<string> Handle(UploadInvestmentPackageLogoCommand request, CancellationToken cancellationToken)
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
                var logo = await _documentDbContext.InvestmentPackageLogos.Include(pl => pl.Contents)
                    .FirstOrDefaultAsync(p => p.PageId == request.PageId && p.InvestmentPackageId == request.InvestmentPackageId, cancellationToken);
                if (logo == null)
                {
                    _logger?.Verbose($"Investment package logo was not found previously. Added new one");
                    await AddNewDocumentAsync(request, fileId, documentUrl);
                }
                else
                {
                    _logger?.Verbose($"Investment package logo existed before. Updating it.");
                    await UpdateExistingDocumentAsync(logo, fileId, documentUrl, container);
                }
                await _documentDbContext.SaveChangesAsync(cancellationToken);
                _logger?.Verbose($"Database record was successfully created");
                _logger?.Verbose($"Attempting to send events");
                await PublishEventsAsync(request.PageId, request.InvestmentPackageId, documentUrl);
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
        
         private Task AddNewDocumentAsync(UploadInvestmentPackageLogoCommand request, string fileId, string documentUrl)
        {
            var logo = new InvestmentPackageLogo
            {
                Title = request.FileName,
                PageId = request.PageId,
                InvestmentPackageId = request.InvestmentPackageId
            };
            _documentDbContext.InvestmentPackageLogos.Add(logo);
            logo.Contents.Add(new DocumentContent
            {
                Language = Base.Constants.Languages.English,
                FileId = fileId,
                DocumentAddress = documentUrl,
                DocumentId = logo.Id,
                Document = logo
            });
            return Task.CompletedTask;
        }

        private async Task UpdateExistingDocumentAsync(InvestmentPackageLogo logo, string fileId, string documentUrl, string container)
        {
            var content = logo.Contents.FirstOrDefault();
            if (content == null)
            {
                content = new DocumentContent
                {
                    Language = Base.Constants.Languages.English,
                    FileId = fileId,
                    DocumentAddress = documentUrl
                };
                logo.Contents.Add(content);
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
                        $"Failed to delete existing investment package logo from {container} - {content.FileId}: {ex.Message}/{ex.StackTrace}");
                }

                content.FileId = fileId;
                content.DocumentAddress = documentUrl;
            }
        }

        private async Task PublishEventsAsync(Guid pageId, Guid investmentPackageId, string logoUrl)
        {
            var payload = new InvestmentPackageLogoUpdated(pageId, investmentPackageId, logoUrl);
            var @event = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, payload);
            await _eventPublisher.PublishAsync(@event);
        }
    }
}