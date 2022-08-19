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

namespace Mosaico.Integration.Blockchain.Ethereum.SignatureCheckerUpgradeable.ContractDefinition
{


    public partial class SignatureCheckerUpgradeableDeployment : SignatureCheckerUpgradeableDeploymentBase
    {
        public SignatureCheckerUpgradeableDeployment() : base(BYTECODE) { }
        public SignatureCheckerUpgradeableDeployment(string byteCode) : base(byteCode) { }
    }

    public class SignatureCheckerUpgradeableDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60566037600b82828239805160001a607314602a57634e487b7160e01b600052600060045260246000fd5b30600052607381538281f3fe73000000000000000000000000000000000000000030146080604052600080fdfea2646970667358221220fdacd08b0ada0197dc6cdce15592f1d8dcc2b9f3723260f723c66eee203fa7bb64736f6c634300080d0033";
        public SignatureCheckerUpgradeableDeploymentBase() : base(BYTECODE) { }
        public SignatureCheckerUpgradeableDeploymentBase(string byteCode) : base(byteCode) { }

    }
}
