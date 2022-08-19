using System;
using System.Numerics;

namespace Mosaico.Integration.Blockchain.Ethereum.Models
{
    public class EthTransaction
    {
        public string From { get; set; }
        public string To { get; set; }
        public string TransactionHash { get; set; }
        public BigInteger Value { get; set; }
        public string BlockHash { get; set; }
        public BigInteger BlockNumber { get; set; }
        public DateTimeOffset FinishedAt { get; set; }
        public string Type { get; set; }
        public TransactionDirectionType DirectionType { get; set; }
        public BigInteger Gas { get; set; }
        public long Status { get; set; }
    }
}