using System;
using Mosaico.Domain.Base;

namespace Mosaico.Domain.Wallet.Entities
{
    public class TokenHolderScanJobLog : EntityBase
    {
        public virtual Token Token { get; set; }
        public Guid TokenId { get; set; }
        public ulong LastFetchedBlock { get; set; }
    }
}