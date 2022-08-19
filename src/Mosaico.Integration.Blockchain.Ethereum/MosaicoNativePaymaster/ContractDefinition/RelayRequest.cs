using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Mosaico.Integration.Blockchain.Ethereum.MosaicoNativePaymaster.ContractDefinition
{
    public partial class RelayRequest : RelayRequestBase { }

    public class RelayRequestBase 
    {
        [Parameter("tuple", "request", 1)]
        public virtual ForwardRequest Request { get; set; }
        [Parameter("tuple", "relayData", 2)]
        public virtual RelayData RelayData { get; set; }
    }
}
