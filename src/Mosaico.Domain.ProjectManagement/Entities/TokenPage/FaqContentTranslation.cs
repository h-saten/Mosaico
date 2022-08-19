using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public class FaqContentTranslation : TranslationBase
    {
        public virtual FaqContent FaqContent { get; set; }
    }
}