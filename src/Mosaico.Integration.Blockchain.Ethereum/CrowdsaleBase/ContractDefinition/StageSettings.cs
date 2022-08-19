using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Mosaico.Integration.Blockchain.Ethereum.CrowdsaleBase.ContractDefinition
{
    public partial class StageSettings : StageSettingsBase { }

    public class StageSettingsBase 
    {
        [Parameter("string", "name", 1)]
        public virtual string Name { get; set; }
        [Parameter("bool", "isPrivate", 2)]
        public virtual bool IsPrivate { get; set; }
        [Parameter("uint256", "cap", 3)]
        public virtual BigInteger Cap { get; set; }
        [Parameter("uint256", "minIndividualCap", 4)]
        public virtual BigInteger MinIndividualCap { get; set; }
        [Parameter("uint256", "maxIndividualCap", 5)]
        public virtual BigInteger MaxIndividualCap { get; set; }
        [Parameter("address[]", "whitelist", 6)]
        public virtual List<string> Whitelist { get; set; }
        [Parameter("uint256", "rate", 7)]
        public virtual BigInteger Rate { get; set; }
    }
}
