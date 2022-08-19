using Mosaico.Domain.Base;
using System;

namespace Mosaico.Domain.DocumentManagement.Entities
{
    public class DocumentContent : EntityBase
    {
        public string FileId { get; set; }
        public string Language { get; set; }
        public string DocumentAddress { get; set; }
        public Guid DocumentId { get; set; }
        public virtual DocumentBase Document { get; set; }
    }
}