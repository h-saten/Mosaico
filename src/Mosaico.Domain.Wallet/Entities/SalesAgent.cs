using System.Collections.Generic;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class SalesAgent : EntityBase
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public virtual List<Transaction> Transactions { get; set; }
    }
}