using System;

namespace Mosaico.Integration.Blockchain.Ethereum.Models
{
    public class Stake
    {
        public virtual string Staker { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual DateTimeOffset Since { get; set; }
        public virtual string Token { get; set; }
        public virtual bool Active { get; set; }
    }
}