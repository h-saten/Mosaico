using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.DocumentManagement.Abstractions;
using Mosaico.Application.DocumentManagement.Commands.Token.UploadTokenLogo;
using Mosaico.Application.DocumentManagement.DTOs;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Domain.DocumentManagement.Entities;
using Mosaico.Storage.Base;
using Serilog;

namespace Mosaico.Application.DocumentManagement.Services
{
    public class DocumentUploadService : IDocumentUploadService
    {
        private readonly IDocumentDbContext _documentDbContext;
        private readonly IStorageClient _storageClient;
        private readonly ILogger _logger;

        public DocumentUploadService(IDocumentDbContext documentDbContext, IStorageClient storageClient, ILogger logger = null)
        {
            _documentDbContext = documentDbContext;
            _storageClient = storageClient;
            _logger = logger;
        }

        public async Task AddNewDocumentAsync<TDocument, TRequest>(TRequest request, string fileId, string documentUrl, string language = Base.Constants.Languages.English) where TDocument : DocumentBase, new() where TRequest : UploadDocumentRequestBase
        {
            var document = new TDocument
            {
                Title = request.FileName
            };
            var relatedEntityId = request.GetEntityId();
            document.SetRelatedEntityId(relatedEntityId);
            document.Contents.Add(new DocumentContent
            {
                Language = language,
                FileId = fileId,
                DocumentAddress = documentUrl,
                DocumentId = document.Id,
                Document = document
            });
            _documentDbContext.Entry(document).State = EntityState.Added;
            await _documentDbContext.SaveChangesAsync();
        }
        
        public async Task UpdateExistingDocumentAsync<TDocument>(TDocument document, string fileId, string documentUrl, string container, string language = Base.Constants.Languages.English) where TDocument : DocumentBase, new()
        {
            var content = document.Contents.FirstOrDefault();
            if (content == null)
            {
                content = new DocumentContent
                {
                    Language = language,
                    FileId = fileId,
                    DocumentAddress = documentUrl,
                    Document = document,
                    DocumentId = document.Id
                };
                document.Contents.Add(content);
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
                        $"Failed to delete existing token logo from {container} - {content.FileId}: {ex.Message}/{ex.StackTrace}");
                }

                content.FileId = fileId;
                content.DocumentAddress = documentUrl;
            }
            await _documentDbContext.SaveChangesAsync();
        }
    }
}