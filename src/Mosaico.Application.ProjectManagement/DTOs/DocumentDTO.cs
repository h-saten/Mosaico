using System;

namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class DocumentDTO
    {
        public Guid? Id { get; set; }
        public Guid? ProjectId { get; set; }
        public string Url { get; set; }
        public DocumentTypesDTO Type { get; set; }
        public string Language { get; set; }
    }
}