using System;

namespace Mosaico.SDK.ProjectManagement.Models
{
    public class MosaicoProjectDocument
    {
        public Guid Id { get; set; }
        public string Language { get; set; }
        public string Content { get; set; }
        public Guid ProjectId { get; set; }
        public string Url { get; set; }
    }
}