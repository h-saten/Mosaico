using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public class SocialMediaLink : TranslatableEntityBase<SocialMediaLinkTranslation>
    {
        public int Order { get; set; }
        public bool IsHidden { get; set; }
        public Guid PageId { get; set; }
        public virtual Page Page { get; set; }
    }
}