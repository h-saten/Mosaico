using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Mosaico.Storage.Base;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Commands.UploadCompanyDocuments
{
    internal class UploadCompanyDocumentsCommandHandler : IRequestHandler<UploadCompanyDocumentsCommand, Guid>
    {
        private readonly ILogger _logger;
        private readonly IBusinessDbContext _businessDbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IStorageClient _storageClient;

        public UploadCompanyDocumentsCommandHandler(IBusinessDbContext businessDbContext, IStorageClient storageClient, ILogger logger, IEventPublisher eventPublisher, IEventFactory eventFactory)
        {
            _logger = logger;
            _businessDbContext = businessDbContext;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _storageClient = storageClient;
        }

        public async Task<Guid> Handle(UploadCompanyDocumentsCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to create company document");

            var company = await _businessDbContext.Companies.FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken);

            if (company == null)
            {
                throw new CompanyNotFoundException(request.CompanyId);
            }

            var fileId = await _storageClient.CreateAsync(new StorageObject
            {
                Container = "ci-documents",
                Content = request.Content,
                Size = request.Content.Length,
                FileName = $"{company.Slug}_{request.Title}.pdf",
                MimeType = "application/pdf"
            });
            var url = await _storageClient.GetFileURLAsync(fileId, "ci-documents");

            var document = new Document
            {
                CompanyId = company.Id,
                Company = company,
                Content = request.Content?.ToString(),
                Language = request.Language,
                Title = request.Title,
                Url = url
            };
            _businessDbContext.Documents.Add(document);

            await _businessDbContext.SaveChangesAsync(cancellationToken);

            await PublishEvents(document.Id, url);
            return document.Id;

        }

        private async Task PublishEvents(Guid documentId, string url)
        {
            var e = _eventFactory.CreateEvent(Events.BusinessManagement.Constants.EventPaths.Companies, new CompanyDocumentUploaded(documentId, url));
            await _eventPublisher.PublishAsync(e);
        }
    }
}
