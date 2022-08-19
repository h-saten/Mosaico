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

namespace Mosaico.Integration.Blockchain.Ethereum.Ownable.ContractDefinition
{


    public partial class OwnableDeployment : OwnableDeploymentBase
    {
        public OwnableDeployment() : base(BYTECODE) { }
        public OwnableDeployment(string byteCode) : base(byteCode) { }
    }

    public class OwnableDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405234801561001057600080fd5b50600080546001600160a01b031916339081178255604051909182917f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e0908290a350610498806100616000396000f3fe608060405234801561001057600080fd5b50600436106100625760003560e01c8063715018a6146100675780638da5cb5b14610071578063a69df4b514610091578063b6c5232414610099578063dd467064146100aa578063f2fde38b146100bd575b600080fd5b61006f6100d0565b005b6000546040516001600160a01b0390911681526020015b60405180910390f35b61006f61013b565b600254604051908152602001610088565b61006f6100b836600461039e565b610241565b61006f6100cb3660046103b7565b6102c6565b6000546001600160a01b031633146101035760405162461bcd60e51b81526004016100fa906103e7565b60405180910390fd5b600080546040516001600160a01b0390911690600080516020610443833981519152908390a3600080546001600160a01b0319169055565b6001546001600160a01b031633146101a15760405162461bcd60e51b815260206004820152602360248201527f596f7520646f6e27742068617665207065726d697373696f6e20746f20756e6c6044820152626f636b60e81b60648201526084016100fa565b60025442116101f25760405162461bcd60e51b815260206004820152601f60248201527f436f6e7472616374206973206c6f636b656420756e74696c203720646179730060448201526064016100fa565b600154600080546040516001600160a01b03938416939091169160008051602061044383398151915291a3600154600080546001600160a01b0319166001600160a01b03909216919091179055565b6000546001600160a01b0316331461026b5760405162461bcd60e51b81526004016100fa906103e7565b60008054600180546001600160a01b03199081166001600160a01b0384161790915516905561029a814261041c565b600255600080546040516001600160a01b0390911690600080516020610443833981519152908390a350565b6000546001600160a01b031633146102f05760405162461bcd60e51b81526004016100fa906103e7565b6001600160a01b0381166103555760405162461bcd60e51b815260206004820152602660248201527f4f776e61626c653a206e6577206f776e657220697320746865207a65726f206160448201526564647265737360d01b60648201526084016100fa565b600080546040516001600160a01b038085169392169160008051602061044383398151915291a3600080546001600160a01b0319166001600160a01b0392909216919091179055565b6000602082840312156103b057600080fd5b5035919050565b6000602082840312156103c957600080fd5b81356001600160a01b03811681146103e057600080fd5b9392505050565b6020808252818101527f4f776e61626c653a2063616c6c6572206973206e6f7420746865206f776e6572604082015260600190565b6000821982111561043d57634e487b7160e01b600052601160045260246000fd5b50019056fe8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e0a2646970667358221220b33ce3ec2f504d3c3ac1c2485a3b0cc9411ec587b0db33c2879862f9b948915064736f6c634300080d0033";
        public OwnableDeploymentBase() : base(BYTECODE) { }
        public OwnableDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class GeUnlockTimeFunction : GeUnlockTimeFunctionBase { }

    [Function("geUnlockTime", "uint256")]
    public class GeUnlockTimeFunctionBase : FunctionMessage
    {

    }

    public partial class LockFunction : LockFunctionBase { }

    [Function("lock")]
    public class LockFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "time", 1)]
        public virtual BigInteger Time { get; set; }
    }

    public partial class OwnerFunction : OwnerFunctionBase { }

    [Function("owner", "address")]
    public class OwnerFunctionBase : FunctionMessage
    {

    }

    public partial class RenounceOwnershipFunction : RenounceOwnershipFunctionBase { }

    [Function("renounceOwnership")]
    public class RenounceOwnershipFunctionBase : FunctionMessage
    {

    }

    public partial class TransferOwnershipFunction : TransferOwnershipFunctionBase { }

    [Function("transferOwnership")]
    public class TransferOwnershipFunctionBase : FunctionMessage
    {
        [Parameter("address", "newOwner", 1)]
        public virtual string NewOwner { get; set; }
    }

    public partial class UnlockFunction : UnlockFunctionBase { }

    [Function("unlock")]
    public class UnlockFunctionBase : FunctionMessage
    {

    }

    public partial class OwnershipTransferredEventDTO : OwnershipTransferredEventDTOBase { }

    [Event("OwnershipTransferred")]
    public class OwnershipTransferredEventDTOBase : IEventDTO
    {
        [Parameter("address", "previousOwner", 1, true )]
        public virtual string PreviousOwner { get; set; }
        [Parameter("address", "newOwner", 2, true )]
        public virtual string NewOwner { get; set; }
    }

    public partial class GeUnlockTimeOutputDTO : GeUnlockTimeOutputDTOBase { }

    [FunctionOutput]
    public class GeUnlockTimeOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class OwnerOutputDTO : OwnerOutputDTOBase { }

    [FunctionOutput]
    public class OwnerOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }






}
