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

namespace Mosaico.Integration.Blockchain.Ethereum.CountersUpgradeable.ContractDefinition
{


    public partial class CountersUpgradeableDeployment : CountersUpgradeableDeploymentBase
    {
        public CountersUpgradeableDeployment() : base(BYTECODE) { }
        public CountersUpgradeableDeployment(string byteCode) : base(byteCode) { }
    }

    public class CountersUpgradeableDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60566037600b82828239805160001a607314602a57634e487b7160e01b600052600060045260246000fd5b30600052607381538281f3fe73000000000000000000000000000000000000000030146080604052600080fdfea26469706673582212200b494f9984ae9c2756b33d1da9d0f609afd2191fc5ef850695b2d7587c6aa72064736f6c634300080d0033";
        public CountersUpgradeableDeploymentBase() : base(BYTECODE) { }
        public CountersUpgradeableDeploymentBase(string byteCode) : base(byteCode) { }

    }
}
