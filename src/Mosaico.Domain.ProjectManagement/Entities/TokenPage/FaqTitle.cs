using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public class FaqTitle : TranslatableEntityBase<FaqTitleTranslation>
    {
        public Guid FaqId { get; set; }
        public virtual Faq Faq { get; set; }
    }
}