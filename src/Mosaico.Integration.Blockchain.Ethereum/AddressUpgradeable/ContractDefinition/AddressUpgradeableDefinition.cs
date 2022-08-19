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

namespace Mosaico.Integration.Blockchain.Ethereum.AddressUpgradeable.ContractDefinition
{


    public partial class AddressUpgradeableDeployment : AddressUpgradeableDeploymentBase
    {
        public AddressUpgradeableDeployment() : base(BYTECODE) { }
        public AddressUpgradeableDeployment(string byteCode) : base(byteCode) { }
    }

    public class AddressUpgradeableDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60566037600b82828239805160001a607314602a57634e487b7160e01b600052600060045260246000fd5b30600052607381538281f3fe73000000000000000000000000000000000000000030146080604052600080fdfea264697066735822122075c406021014faefb25c8240183b0e7c4f93d53f79cf61b83d7b336efc17a4b464736f6c634300080d0033";
        public AddressUpgradeableDeploymentBase() : base(BYTECODE) { }
        public AddressUpgradeableDeploymentBase(string byteCode) : base(byteCode) { }

    }
}
