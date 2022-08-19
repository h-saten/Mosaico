using System.Numerics;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public class DaoConfiguration
    {
        public string Name { get; set; }
        public BigInteger InitialVotingDelay { get; set; }
        public BigInteger InitialVotingPeriod { get; set; }
        public BigInteger Quorum { get; set; }
        public string Owner { get; set; }
        public bool IsVotingEnabled { get; set; }
        public bool OnlyOwnerProposals { get; set; }
        public string PrivateKey { get; set; }
    }
}