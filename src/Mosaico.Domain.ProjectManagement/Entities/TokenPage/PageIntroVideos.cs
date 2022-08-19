using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public class PageIntroVideos : EntityBase
    {
        public string VideoUrl { get; set; }
        public string VideoExternalLink { get; set; }
        public bool ShowLocalVideo { get; set; }
        public bool IsExternalLink { get; set; }
        public virtual Page Page { get; set; }
        public Guid PageId { get; set; }
    }
}
