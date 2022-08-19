using Mosaico.Domain.Base;
using System;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class DocumentType : EntityBase
    {
        public string Title { get; set; }
        public string Key { get; set; }
        public int Order { get; set; }
        public DocumentType(string key, string title)
        {
            Id = Guid.NewGuid();
            Key = key;
            Title = title;
        }
    }
}