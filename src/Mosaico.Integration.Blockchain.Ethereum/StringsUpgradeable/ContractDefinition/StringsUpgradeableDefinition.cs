using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace Mosaico.Integration.Blockchain.Ethereum.StringsUpgradeable.ContractDefinition
{


    public partial class StringsUpgradeableDeployment : StringsUpgradeableDeploymentBase
    {
        public StringsUpgradeableDeployment() : base(BYTECODE) { }
        public StringsUpgradeableDeployment(string byteCode) : base(byteCode) { }
    }

    public class StringsUpgradeableDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60566037600b82828239805160001a607314602a57634e487b7160e01b600052600060045260246000fd5b30600052607381538281f3fe73000000000000000000000000000000000000000030146080604052600080fdfea2646970667358221220f6a2b2b6769e9831fb093379a62a3e932e144cec31b0b379bab29b04b503217a64736f6c634300080d0033";
        public StringsUpgradeableDeploymentBase() : base(BYTECODE) { }
        public StringsUpgradeableDeploymentBase(string byteCode) : base(byteCode) { }

    }
}
