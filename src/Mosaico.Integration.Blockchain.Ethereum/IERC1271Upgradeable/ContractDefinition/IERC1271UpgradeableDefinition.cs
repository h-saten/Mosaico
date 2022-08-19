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

namespace Mosaico.Integration.Blockchain.Ethereum.IERC1271Upgradeable.ContractDefinition
{


    public partial class IERC1271UpgradeableDeployment : IERC1271UpgradeableDeploymentBase
    {
        public IERC1271UpgradeableDeployment() : base(BYTECODE) { }
        public IERC1271UpgradeableDeployment(string byteCode) : base(byteCode) { }
    }

    public class IERC1271UpgradeableDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public IERC1271UpgradeableDeploymentBase() : base(BYTECODE) { }
        public IERC1271UpgradeableDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class IsValidSignatureFunction : IsValidSignatureFunctionBase { }

    [Function("isValidSignature", "bytes4")]
    public class IsValidSignatureFunctionBase : FunctionMessage
    {
        [Parameter("bytes32", "hash", 1)]
        public virtual byte[] Hash { get; set; }
        [Parameter("bytes", "signature", 2)]
        public virtual byte[] Signature { get; set; }
    }

    public partial class IsValidSignatureOutputDTO : IsValidSignatureOutputDTOBase { }

    [FunctionOutput]
    public class IsValidSignatureOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes4", "magicValue", 1)]
        public virtual byte[] MagicValue { get; set; }
    }
}
