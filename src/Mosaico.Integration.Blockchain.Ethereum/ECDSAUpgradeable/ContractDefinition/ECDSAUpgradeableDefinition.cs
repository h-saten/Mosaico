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

namespace Mosaico.Integration.Blockchain.Ethereum.ECDSAUpgradeable.ContractDefinition
{


    public partial class ECDSAUpgradeableDeployment : ECDSAUpgradeableDeploymentBase
    {
        public ECDSAUpgradeableDeployment() : base(BYTECODE) { }
        public ECDSAUpgradeableDeployment(string byteCode) : base(byteCode) { }
    }

    public class ECDSAUpgradeableDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60566037600b82828239805160001a607314602a57634e487b7160e01b600052600060045260246000fd5b30600052607381538281f3fe73000000000000000000000000000000000000000030146080604052600080fdfea264697066735822122074f31095bcb09969520d205201b78fbe1b9205ba7e0f0c7427ecaf0232acd13c64736f6c634300080d0033";
        public ECDSAUpgradeableDeploymentBase() : base(BYTECODE) { }
        public ECDSAUpgradeableDeploymentBase(string byteCode) : base(byteCode) { }

    }
}
