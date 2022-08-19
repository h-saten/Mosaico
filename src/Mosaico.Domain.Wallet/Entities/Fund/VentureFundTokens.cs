using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities.Fund
{
    public class VentureFundToken : EntityBase
    {
        public bool Hidden { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Logo { get; set; }
        public bool IsStakingEnabled { get; set; }
        public decimal LatestPrice { get; set; }
        public Guid VentureFundId { get; set; }
        public virtual VentureFund VentureFund { get; set; }
    }
}