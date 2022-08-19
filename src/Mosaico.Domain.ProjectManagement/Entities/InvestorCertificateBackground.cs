using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class InvestorCertificateBackground : EntityBase
    {
        public string Language { get; set; }
        public string Url { get; set; }
        public Guid InvestorCertificateId { get; set; }
        public virtual InvestorCertificate InvestorCertificate { get; set; }
    }
}