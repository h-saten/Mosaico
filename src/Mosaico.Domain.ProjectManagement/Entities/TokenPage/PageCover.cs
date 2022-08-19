using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public class PageCover : TranslatableEntityBase<PageCoverTranslation>
    {
        public PageCover()
        {
            Title = Guid.NewGuid().ToString();
            Key = Title;
        }
        
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public string CoverColor { get; set; }
        public Guid PageId { get; set; }
        public virtual Page Page { get; set; }
    }
}