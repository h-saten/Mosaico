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

namespace Mosaico.Integration.Blockchain.Ethereum.MosaicoNativePaymaster.ContractDefinition
{


    public partial class MosaicoNativePaymasterDeployment : MosaicoNativePaymasterDeploymentBase
    {
        public MosaicoNativePaymasterDeployment() : base(BYTECODE) { }
        public MosaicoNativePaymasterDeployment(string byteCode) : base(byteCode) { }
    }

    public class MosaicoNativePaymasterDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405234801561001057600080fd5b5061001a3361001f565b61006f565b600080546001600160a01b038381166001600160a01b0319831681178455604051919092169283917f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e09190a35050565b6110698061007e6000396000f3fe6080604052600436106101385760003560e01c80638da5cb5b116100ab578063b90b41cf1161006f578063b90b41cf1461046a578063bbdaa3c914610480578063da74222814610497578063df463a66146104b7578063f2fde38b146104cc578063f9c002f7146104ec57600080fd5b80638da5cb5b146103905780638e54a4d7146103ae578063921276ea146103ce578063a5dcd07b14610402578063b039a88f1461042257600080fd5b8063715018a6116100fd578063715018a6146102cb57806374e861d6146102e057806376fa01c314610312578063776d1a01146103325780637bb05264146103525780637da0a8771461037257600080fd5b8062be5dd4146101fb5780632afe31c1146102325780632d14c4b714610255578063562c4784146102755780635c5e3db1146102b557600080fd5b366101f6576001546001600160a01b031661019a5760405162461bcd60e51b815260206004820152601960248201527f72656c6179206875622061646472657373206e6f74207365740000000000000060448201526064015b60405180910390fd5b60015460405163aa67c91960e01b81523060048201526001600160a01b039091169063aa67c9199034906024016000604051808303818588803b1580156101e057600080fd5b505af11580156101f4573d6000803e3d6000fd5b005b600080fd5b34801561020757600080fd5b5061021b610216366004610ccd565b610503565b604051610229929190610dc6565b60405180910390f35b34801561023e57600080fd5b506102476105b7565b604051908152602001610229565b34801561026157600080fd5b506101f4610270366004610dff565b610629565b34801561028157600080fd5b506102a5610290366004610e2f565b60036020526000908152604090205460ff1681565b6040519015158152602001610229565b3480156102c157600080fd5b5061024761290481565b3480156102d757600080fd5b506101f46106bb565b3480156102ec57600080fd5b506001546001600160a01b03165b6040516001600160a01b039091168152602001610229565b34801561031e57600080fd5b506101f461032d366004610e61565b6106f1565b34801561033e57600080fd5b506101f461034d366004610e2f565b610736565b34801561035e57600080fd5b506101f461036d366004610e2f565b6107ba565b34801561037e57600080fd5b506002546001600160a01b03166102fa565b34801561039c57600080fd5b506000546001600160a01b03166102fa565b3480156103ba57600080fd5b506101f46103c9366004610e2f565b610806565b3480156103da57600080fd5b506040805180820182526005815264191719171b60d91b602082015290516102299190610ef1565b34801561040e57600080fd5b506101f461041d366004610f04565b610851565b34801561042e57600080fd5b506104376108d8565b60405161022991908151815260208083015190820152604080830151908201526060918201519181019190915260800190565b34801561047657600080fd5b5061024761c35081565b34801561048c57600080fd5b506102476201adb081565b3480156104a357600080fd5b506101f46104b2366004610e2f565b610941565b3480156104c357600080fd5b5061024761098d565b3480156104d857600080fd5b506101f46104e7366004610e2f565b61099f565b3480156104f857600080fd5b50610247620186a081565b6060600061051088610851565b6003600061051e8a80610f41565b61052f906040810190602001610e2f565b6001600160a01b0316815260208101919091526040016000205460ff16151560011461055a57600080fd5b6040514281527f1fab8d24eefec3578055fc7f1c7cce1b10153c6731e0b5ca8147e8916d884b459060200160405180910390a1604080514260208201520160408051601f1981840301815291905298600098509650505050505050565b6001546040516370a0823160e01b81523060048201526000916001600160a01b0316906370a0823190602401602060405180830381865afa158015610600573d6000803e3d6000fd5b505050506040513d601f19601f820116820180604052508101906106249190610f61565b905090565b6000546001600160a01b031633146106535760405162461bcd60e51b815260040161019190610f7a565b600154604051627b8a6760e11b8152600481018490526001600160a01b0383811660248301529091169062f714ce90604401600060405180830381600087803b15801561069f57600080fd5b505af11580156106b3573d6000803e3d6000fd5b505050505050565b6000546001600160a01b031633146106e55760405162461bcd60e51b815260040161019190610f7a565b6106ef6000610a33565b565b7f142c6d2c03c5b3fcbb162bc0e230bbaae8c9032eb624a8fd44658e00efa63c9061071e85870187610faf565b60405190815260200160405180910390a15050505050565b6000546001600160a01b031633146107605760405162461bcd60e51b815260040161019190610f7a565b6001600160a01b038116600081815260036020908152604091829020805460ff1916600117905590519182527f3bfb4bbf112628248058745a3c57e35b13369386e474b8e56c552f3063a4a196910160405180910390a150565b6000546001600160a01b031633146107e45760405162461bcd60e51b815260040161019190610f7a565b600180546001600160a01b0319166001600160a01b0392909216919091179055565b6000546001600160a01b031633146108305760405162461bcd60e51b815260040161019190610f7a565b6001600160a01b03166000908152600360205260409020805460ff19169055565b61085e6020820182610fc8565b61086f9060c081019060a001610e2f565b6002546001600160a01b039081169116146108cc5760405162461bcd60e51b815260206004820152601860248201527f466f72776172646572206973206e6f74207472757374656400000000000000006044820152606401610191565b6108d581610a83565b50565b6109036040518060800160405280600081526020016000815260200160008152602001600081525090565b604051806080016040528061c350620dbba061091f9190610fde565b8152602001620dbba08152602001620dbba0815260200161ec54815250905090565b6000546001600160a01b0316331461096b5760405162461bcd60e51b815260040161019190610f7a565b600280546001600160a01b0319166001600160a01b0392909216919091179055565b61099c61c350620186a0610fde565b81565b6000546001600160a01b031633146109c95760405162461bcd60e51b815260040161019190610f7a565b6001600160a01b038116610a2e5760405162461bcd60e51b815260206004820152602660248201527f4f776e61626c653a206e6577206f776e657220697320746865207a65726f206160448201526564647265737360d01b6064820152608401610191565b6108d5815b600080546001600160a01b038381166001600160a01b0319831681178455604051919092169283917f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e09190a35050565b600080610a908380610f41565b610aa1906040810190602001610e2f565b6001600160a01b031663572b6c0560e01b610abf6020860186610fc8565b610ad09060c081019060a001610e2f565b6040516001600160a01b03909116602482015260440160408051601f198184030181529181526020820180516001600160e01b03166001600160e01b0319909416939093179092529051610b249190611004565b600060405180830381855afa9150503d8060008114610b5f576040519150601f19603f3d011682016040523d82523d6000602084013e610b64565b606091505b509150915081610bb65760405162461bcd60e51b815260206004820152601c60248201527f697354727573746564466f727761726465723a207265766572746564000000006044820152606401610191565b8051602014610c075760405162461bcd60e51b815260206004820181905260248201527f697354727573746564466f727761726465723a2062616420726573706f6e73656044820152606401610191565b80806020019051810190610c1b9190611016565b610c675760405162461bcd60e51b815260206004820152601f60248201527f696e76616c696420666f7277617264657220666f7220726563697069656e74006044820152606401610191565b505050565b600060408284031215610c7e57600080fd5b50919050565b60008083601f840112610c9657600080fd5b50813567ffffffffffffffff811115610cae57600080fd5b602083019150836020828501011115610cc657600080fd5b9250929050565b60008060008060008060808789031215610ce657600080fd5b863567ffffffffffffffff80821115610cfe57600080fd5b610d0a8a838b01610c6c565b97506020890135915080821115610d2057600080fd5b610d2c8a838b01610c84565b90975095506040890135915080821115610d4557600080fd5b50610d5289828a01610c84565b979a9699509497949695606090950135949350505050565b60005b83811015610d85578181015183820152602001610d6d565b83811115610d94576000848401525b50505050565b60008151808452610db2816020860160208601610d6a565b601f01601f19169290920160200192915050565b604081526000610dd96040830185610d9a565b905082151560208301529392505050565b6001600160a01b03811681146108d557600080fd5b60008060408385031215610e1257600080fd5b823591506020830135610e2481610dea565b809150509250929050565b600060208284031215610e4157600080fd5b8135610e4c81610dea565b9392505050565b80151581146108d557600080fd5b600080600080600060808688031215610e7957600080fd5b853567ffffffffffffffff80821115610e9157600080fd5b610e9d89838a01610c84565b909750955060208801359150610eb282610e53565b9093506040870135925060608701359080821115610ecf57600080fd5b5086016101008189031215610ee357600080fd5b809150509295509295909350565b602081526000610e4c6020830184610d9a565b600060208284031215610f1657600080fd5b813567ffffffffffffffff811115610f2d57600080fd5b610f3984828501610c6c565b949350505050565b6000823560de19833603018112610f5757600080fd5b9190910192915050565b600060208284031215610f7357600080fd5b5051919050565b6020808252818101527f4f776e61626c653a2063616c6c6572206973206e6f7420746865206f776e6572604082015260600190565b600060208284031215610fc157600080fd5b5035919050565b6000823560fe19833603018112610f5757600080fd5b60008219821115610fff57634e487b7160e01b600052601160045260246000fd5b500190565b60008251610f57818460208701610d6a565b60006020828403121561102857600080fd5b8151610e4c81610e5356fea2646970667358221220dc05a5178f7f56c860755c8c56ad6fa5bb672816c211b3d3bc594b993163a0cd64736f6c634300080f0033";
        public MosaicoNativePaymasterDeploymentBase() : base(BYTECODE) { }
        public MosaicoNativePaymasterDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class CalldataSizeLimitFunction : CalldataSizeLimitFunctionBase { }

    [Function("CALLDATA_SIZE_LIMIT", "uint256")]
    public class CalldataSizeLimitFunctionBase : FunctionMessage
    {

    }

    public partial class ForwarderHubOverheadFunction : ForwarderHubOverheadFunctionBase { }

    [Function("FORWARDER_HUB_OVERHEAD", "uint256")]
    public class ForwarderHubOverheadFunctionBase : FunctionMessage
    {

    }

    public partial class PaymasterAcceptanceBudgetFunction : PaymasterAcceptanceBudgetFunctionBase { }

    [Function("PAYMASTER_ACCEPTANCE_BUDGET", "uint256")]
    public class PaymasterAcceptanceBudgetFunctionBase : FunctionMessage
    {

    }

    public partial class PostRelayedCallGasLimitFunction : PostRelayedCallGasLimitFunctionBase { }

    [Function("POST_RELAYED_CALL_GAS_LIMIT", "uint256")]
    public class PostRelayedCallGasLimitFunctionBase : FunctionMessage
    {

    }

    public partial class PreRelayedCallGasLimitFunction : PreRelayedCallGasLimitFunctionBase { }

    [Function("PRE_RELAYED_CALL_GAS_LIMIT", "uint256")]
    public class PreRelayedCallGasLimitFunctionBase : FunctionMessage
    {

    }

    public partial class GetGasAndDataLimitsFunction : GetGasAndDataLimitsFunctionBase { }

    [Function("getGasAndDataLimits", typeof(GetGasAndDataLimitsOutputDTO))]
    public class GetGasAndDataLimitsFunctionBase : FunctionMessage
    {

    }

    public partial class GetHubAddrFunction : GetHubAddrFunctionBase { }

    [Function("getHubAddr", "address")]
    public class GetHubAddrFunctionBase : FunctionMessage
    {

    }

    public partial class GetRelayHubDepositFunction : GetRelayHubDepositFunctionBase { }

    [Function("getRelayHubDeposit", "uint256")]
    public class GetRelayHubDepositFunctionBase : FunctionMessage
    {

    }

    public partial class OwnerFunction : OwnerFunctionBase { }

    [Function("owner", "address")]
    public class OwnerFunctionBase : FunctionMessage
    {

    }

    public partial class PostRelayedCallFunction : PostRelayedCallFunctionBase { }

    [Function("postRelayedCall")]
    public class PostRelayedCallFunctionBase : FunctionMessage
    {
        [Parameter("bytes", "context", 1)]
        public virtual byte[] Context { get; set; }
        [Parameter("bool", "success", 2)]
        public virtual bool Success { get; set; }
        [Parameter("uint256", "gasUseWithoutPost", 3)]
        public virtual BigInteger GasUseWithoutPost { get; set; }
        [Parameter("tuple", "relayData", 4)]
        public virtual RelayData RelayData { get; set; }
    }

    public partial class PreRelayedCallFunction : PreRelayedCallFunctionBase { }

    [Function("preRelayedCall", typeof(PreRelayedCallOutputDTO))]
    public class PreRelayedCallFunctionBase : FunctionMessage
    {
        [Parameter("tuple", "relayRequest", 1)]
        public virtual RelayRequest RelayRequest { get; set; }
        [Parameter("bytes", "signature", 2)]
        public virtual byte[] Signature { get; set; }
        [Parameter("bytes", "approvalData", 3)]
        public virtual byte[] ApprovalData { get; set; }
        [Parameter("uint256", "maxPossibleGas", 4)]
        public virtual BigInteger MaxPossibleGas { get; set; }
    }

    public partial class RenounceOwnershipFunction : RenounceOwnershipFunctionBase { }

    [Function("renounceOwnership")]
    public class RenounceOwnershipFunctionBase : FunctionMessage
    {

    }

    public partial class SetRelayHubFunction : SetRelayHubFunctionBase { }

    [Function("setRelayHub")]
    public class SetRelayHubFunctionBase : FunctionMessage
    {
        [Parameter("address", "hub", 1)]
        public virtual string Hub { get; set; }
    }

    public partial class SetTargetFunction : SetTargetFunctionBase { }

    [Function("setTarget")]
    public class SetTargetFunctionBase : FunctionMessage
    {
        [Parameter("address", "target", 1)]
        public virtual string Target { get; set; }
    }

    public partial class SetTrustedForwarderFunction : SetTrustedForwarderFunctionBase { }

    [Function("setTrustedForwarder")]
    public class SetTrustedForwarderFunctionBase : FunctionMessage
    {
        [Parameter("address", "forwarder", 1)]
        public virtual string Forwarder { get; set; }
    }

    public partial class TargetsFunction : TargetsFunctionBase { }

    [Function("targets", "bool")]
    public class TargetsFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class TransferOwnershipFunction : TransferOwnershipFunctionBase { }

    [Function("transferOwnership")]
    public class TransferOwnershipFunctionBase : FunctionMessage
    {
        [Parameter("address", "newOwner", 1)]
        public virtual string NewOwner { get; set; }
    }

    public partial class TrustedForwarderFunction : TrustedForwarderFunctionBase { }

    [Function("trustedForwarder", "address")]
    public class TrustedForwarderFunctionBase : FunctionMessage
    {

    }

    public partial class UnsetTargetFunction : UnsetTargetFunctionBase { }

    [Function("unsetTarget")]
    public class UnsetTargetFunctionBase : FunctionMessage
    {
        [Parameter("address", "target", 1)]
        public virtual string Target { get; set; }
    }

    public partial class VersionPaymasterFunction : VersionPaymasterFunctionBase { }

    [Function("versionPaymaster", "string")]
    public class VersionPaymasterFunctionBase : FunctionMessage
    {

    }

    public partial class WithdrawRelayHubDepositToFunction : WithdrawRelayHubDepositToFunctionBase { }

    [Function("withdrawRelayHubDepositTo")]
    public class WithdrawRelayHubDepositToFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amount", 1)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("address", "target", 2)]
        public virtual string Target { get; set; }
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

    public partial class PostRelayedEventDTO : PostRelayedEventDTOBase { }

    [Event("PostRelayed")]
    public class PostRelayedEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "", 1, false )]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class PreRelayedEventDTO : PreRelayedEventDTOBase { }

    [Event("PreRelayed")]
    public class PreRelayedEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "", 1, false )]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class TargetSetEventDTO : TargetSetEventDTOBase { }

    [Event("TargetSet")]
    public class TargetSetEventDTOBase : IEventDTO
    {
        [Parameter("address", "target", 1, false )]
        public virtual string Target { get; set; }
    }

    public partial class CalldataSizeLimitOutputDTO : CalldataSizeLimitOutputDTOBase { }

    [FunctionOutput]
    public class CalldataSizeLimitOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class ForwarderHubOverheadOutputDTO : ForwarderHubOverheadOutputDTOBase { }

    [FunctionOutput]
    public class ForwarderHubOverheadOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class PaymasterAcceptanceBudgetOutputDTO : PaymasterAcceptanceBudgetOutputDTOBase { }

    [FunctionOutput]
    public class PaymasterAcceptanceBudgetOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class PostRelayedCallGasLimitOutputDTO : PostRelayedCallGasLimitOutputDTOBase { }

    [FunctionOutput]
    public class PostRelayedCallGasLimitOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class PreRelayedCallGasLimitOutputDTO : PreRelayedCallGasLimitOutputDTOBase { }

    [FunctionOutput]
    public class PreRelayedCallGasLimitOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetGasAndDataLimitsOutputDTO : GetGasAndDataLimitsOutputDTOBase { }

    [FunctionOutput]
    public class GetGasAndDataLimitsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("tuple", "limits", 1)]
        public virtual GasAndDataLimits Limits { get; set; }
    }

    public partial class GetHubAddrOutputDTO : GetHubAddrOutputDTOBase { }

    [FunctionOutput]
    public class GetHubAddrOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class GetRelayHubDepositOutputDTO : GetRelayHubDepositOutputDTOBase { }

    [FunctionOutput]
    public class GetRelayHubDepositOutputDTOBase : IFunctionOutputDTO 
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



    public partial class PreRelayedCallOutputDTO : PreRelayedCallOutputDTOBase { }

    [FunctionOutput]
    public class PreRelayedCallOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bytes", "context", 1)]
        public virtual byte[] Context { get; set; }
        [Parameter("bool", "", 2)]
        public virtual bool ReturnValue2 { get; set; }
    }









    public partial class TargetsOutputDTO : TargetsOutputDTOBase { }

    [FunctionOutput]
    public class TargetsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }



    public partial class TrustedForwarderOutputDTO : TrustedForwarderOutputDTOBase { }

    [FunctionOutput]
    public class TrustedForwarderOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }



    public partial class VersionPaymasterOutputDTO : VersionPaymasterOutputDTOBase { }

    [FunctionOutput]
    public class VersionPaymasterOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }


}
