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

namespace Mosaico.Integration.Blockchain.Ethereum.EIP712Upgradeable.ContractDefinition
{


    public partial class EIP712UpgradeableDeployment : EIP712UpgradeableDeploymentBase
    {
        public EIP712UpgradeableDeployment() : base(BYTECODE) { }
        public EIP712UpgradeableDeployment(string byteCode) : base(byteCode) { }
    }

    public class EIP712UpgradeableDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public EIP712UpgradeableDeploymentBase() : base(BYTECODE) { }
        public EIP712UpgradeableDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class InitializedEventDTO : InitializedEventDTOBase { }

    [Event("Initialized")]
    public class InitializedEventDTOBase : IEventDTO
    {
        [Parameter("uint8", "version", 1, false )]
        public virtual byte Version { get; set; }
    }
}
