using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.BusinessManagement.Entities
{
    public class Verification : EntityBase
    {
        public string CompanyRegistrationUrl { get; set; }
        public string CompanyAddressUrl { get; set; }

        public virtual List<Shareholder> Shareholders { get; set; } = new List<Shareholder>();
        public Guid CompanyId { get; set; }
    }
}