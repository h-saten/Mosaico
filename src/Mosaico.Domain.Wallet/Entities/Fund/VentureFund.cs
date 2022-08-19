using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities.Fund
{
    public class VentureFund : EntityBase
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public virtual List<VentureFundToken> Tokens { get; set; } = new List<VentureFundToken>();
        public DateTimeOffset? LastUpdatedAt { get; set; }
    }
}