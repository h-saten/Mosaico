using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public class AboutContent : TranslatableEntityBase<AboutContentTranslation>
    {
        public Guid AboutId { get; set; }
        public virtual About About { get; set; }
    }
}