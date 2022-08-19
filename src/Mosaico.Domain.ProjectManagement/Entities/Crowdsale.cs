using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.ProjectManagement.Entities
{
    public class Crowdsale : EntityBase
    {
        public string Network { get; set; }
        public string ContractAddress { get; set; }
        public string OwnerAddress { get; set; }
        public string ContractVersion { get; set; }
        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }
        public decimal HardCap { get; set; }
        public decimal SoftCap { get; set; }
        public List<string> SupportedStableCoins { get; set; } = new List<string>();
    }
}