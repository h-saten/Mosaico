using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Mosaico.Integration.Blockchain.Ethereum.MosaicoVaultv1.ContractDefinition
{
    public partial class Items : ItemsBase { }

    public class ItemsBase 
    {
        [Parameter("address", "token", 1)]
        public virtual string Token { get; set; }
        [Parameter("address", "withdrawer", 2)]
        public virtual string Withdrawer { get; set; }
        [Parameter("address", "creator", 3)]
        public virtual string Creator { get; set; }
        [Parameter("uint256", "amount", 4)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("uint256", "unlockTimestamp", 5)]
        public virtual BigInteger UnlockTimestamp { get; set; }
        [Parameter("bool", "withdrawn", 6)]
        public virtual bool Withdrawn { get; set; }
        [Parameter("bool", "deposited", 7)]
        public virtual bool Deposited { get; set; }
    }
}
