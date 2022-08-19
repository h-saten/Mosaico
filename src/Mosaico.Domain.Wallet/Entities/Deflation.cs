using System;
using Mosaico.Domain.Base;
using Mosaico.Domain.Wallet.Entities.Enums;

namespace Mosaico.Domain.Wallet.Entities
{
    public class Deflation : EntityBase
    {
        public Guid TokenId { get; set; }
        public virtual Token Token { get; set; }
        public decimal? TransactionPercentage { get; set; }
        public DeflationType Type { get; set; }
        public long? BuyoutDelayInDays { get; set; }
        public decimal? BuyoutPercentage { get; set; }
        public DateTimeOffset? StartsAt { get; set; }
    }
}