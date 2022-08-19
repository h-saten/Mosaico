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

namespace Mosaico.Integration.Blockchain.Ethereum.Initializable.ContractDefinition
{


    public partial class InitializableDeployment : InitializableDeploymentBase
    {
        public InitializableDeployment() : base(BYTECODE) { }
        public InitializableDeployment(string byteCode) : base(byteCode) { }
    }

    public class InitializableDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public InitializableDeploymentBase() : base(BYTECODE) { }
        public InitializableDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class InitializedEventDTO : InitializedEventDTOBase { }

    [Event("Initialized")]
    public class InitializedEventDTOBase : IEventDTO
    {
        [Parameter("uint8", "version", 1, false )]
        public virtual byte Version { get; set; }
    }
}
