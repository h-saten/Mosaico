using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class Article : EntityBase
    {
        public string VisibleText { get; set; }
        public string AuthorPhoto { get; set; }
        public string CoverPicture { get; set; }
        public DateTimeOffset? Date { get; set; } 
        public string Link { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public string AuthorName { get; set; }
        public bool Hidden { get; set; }
    }
}