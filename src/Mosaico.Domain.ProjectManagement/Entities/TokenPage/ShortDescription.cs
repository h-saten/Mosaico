using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public class ShortDescription : TranslatableEntityBase<ShortDescriptionTranslation>
    {
        public ShortDescription()
        {
            Key = Guid.NewGuid().ToString();
            Title = Key;
        }
        
        public Guid PageId { get; set; }
        public virtual Page Page { get; set; }
    }
}