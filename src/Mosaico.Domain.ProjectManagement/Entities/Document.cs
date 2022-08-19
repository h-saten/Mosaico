using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class Document : EntityBase
    {
        public string Language { get; set; }
        public virtual DocumentType Type { get; set; }
        public Guid TypeId { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
    }
}