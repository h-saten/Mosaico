using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Mosaico.Integration.Blockchain.Ethereum.MosaicoNativePaymaster.ContractDefinition
{
    public partial class RelayData : RelayDataBase { }

    public class RelayDataBase 
    {
        [Parameter("uint256", "gasPrice", 1)]
        public virtual BigInteger GasPrice { get; set; }
        [Parameter("uint256", "pctRelayFee", 2)]
        public virtual BigInteger PctRelayFee { get; set; }
        [Parameter("uint256", "baseRelayFee", 3)]
        public virtual BigInteger BaseRelayFee { get; set; }
        [Parameter("address", "relayWorker", 4)]
        public virtual string RelayWorker { get; set; }
        [Parameter("address", "paymaster", 5)]
        public virtual string Paymaster { get; set; }
        [Parameter("address", "forwarder", 6)]
        public virtual string Forwarder { get; set; }
        [Parameter("bytes", "paymasterData", 7)]
        public virtual byte[] PaymasterData { get; set; }
        [Parameter("uint256", "clientId", 8)]
        public virtual BigInteger ClientId { get; set; }
    }
}
