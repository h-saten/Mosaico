using System.Threading.Tasks;
using Mosaico.Application.DocumentManagement.DTOs;
using Mosaico.Domain.DocumentManagement.Entities;

namespace Mosaico.Application.DocumentManagement.Abstractions
{
    public interface IDocumentUploadService
    {
        Task UpdateExistingDocumentAsync<TDocument>(TDocument document, string fileId, string documentUrl, string container, string language = Base.Constants.Languages.English) where TDocument : DocumentBase, new();

        Task AddNewDocumentAsync<TDocument, TRequest>(TRequest request, string fileId, string documentUrl, string language = Base.Constants.Languages.English)
            where TDocument : DocumentBase, new() where TRequest : UploadDocumentRequestBase;
    }
}