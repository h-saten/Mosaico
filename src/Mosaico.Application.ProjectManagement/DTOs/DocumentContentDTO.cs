using System;

namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class DocumentContentDTO
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public DocumentTypesDTO Type { get; set; }
        public string Language { get; set; }
        public string Content { get; set; }
    }
}