using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public class FaqTitleTranslation : TranslationBase
    {
        public virtual FaqTitle FaqTitle { get; set; }
    }
}