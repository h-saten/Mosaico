using System;
using System.Numerics;

namespace Mosaico.Blockchain.Base.DAL.Models
{
    public class ERC20Transfer
    {
        public string TransactionHash { get; set; }
        public string Address { get; set; }
        public DateTimeOffset Date { get; set; }
        public BigInteger BlockNumber { get; set; }
        public string BlockHash { get; set; }
        public string ToAddress { get; set; }
        public string FromAddress { get; set; }
        public decimal Value { get; set; }
    }
}