using System;
using MediatR;

namespace Mosaico.Application.DocumentManagement.DTOs
{
    public abstract class UploadDocumentRequestBase : IRequest<string>
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public abstract Guid GetEntityId();
        public string Language { get; set; } = Base.Constants.Languages.English;
    }
}