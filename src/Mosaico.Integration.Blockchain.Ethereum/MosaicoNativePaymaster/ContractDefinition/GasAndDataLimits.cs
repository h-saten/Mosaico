using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Mosaico.Integration.Blockchain.Ethereum.MosaicoNativePaymaster.ContractDefinition
{
    public partial class GasAndDataLimits : GasAndDataLimitsBase { }

    public class GasAndDataLimitsBase 
    {
        [Parameter("uint256", "acceptanceBudget", 1)]
        public virtual BigInteger AcceptanceBudget { get; set; }
        [Parameter("uint256", "preRelayedCallGasLimit", 2)]
        public virtual BigInteger PreRelayedCallGasLimit { get; set; }
        [Parameter("uint256", "postRelayedCallGasLimit", 3)]
        public virtual BigInteger PostRelayedCallGasLimit { get; set; }
        [Parameter("uint256", "calldataSizeLimit", 4)]
        public virtual BigInteger CalldataSizeLimit { get; set; }
    }
}
