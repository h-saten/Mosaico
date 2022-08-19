using System;

namespace Mosaico.SDK.DocumentManagement.Models
{
    public class ProjectDocument
    {
        public Guid ProjectId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
    }
}