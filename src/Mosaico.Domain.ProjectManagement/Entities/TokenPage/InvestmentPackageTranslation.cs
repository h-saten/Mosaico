using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public class InvestmentPackageTranslation : TranslationBase
    {
        public string Name { get; set; }
        public string Benefits { get; set; }
        public virtual InvestmentPackage InvestmentPackage { get; set; }
    }
}