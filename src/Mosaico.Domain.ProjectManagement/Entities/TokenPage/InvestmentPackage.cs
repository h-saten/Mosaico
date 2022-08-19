using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities.TokenPage
{
    public class InvestmentPackage : TranslatableEntityBase<InvestmentPackageTranslation>
    {
        public InvestmentPackage()
        {
            Key = Guid.NewGuid().ToString();
            Title = Key;
        }
        public Guid PageId { get; set; }
        public virtual Page Page { get; set; }
        public string LogoUrl { get; set; }
        public decimal TokenAmount { get; set; }
    }
}