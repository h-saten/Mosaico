using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public class InvestorCertificateTranslation : TranslationBase
    {
        public virtual InvestorCertificate InvestorCertificate { get; set; }
    }
}