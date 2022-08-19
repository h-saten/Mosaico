using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

namespace Mosaico.Integration.Blockchain.Ethereum.Tether.TetherToken.ContractDefinition
{


    public partial class TetherTokenDeployment : TetherTokenDeploymentBase
    {
        public TetherTokenDeployment() : base(BYTECODE) { }
        public TetherTokenDeployment(string byteCode) : base(byteCode) { }
    }

    public class TetherTokenDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "60806040526000805460a060020a60ff021916815560038190556004553480156200002957600080fd5b506040516200183238038062001832833981016040908152815160208084015192840151606085015160008054600160a060020a03191633179055600184905593850180519395909491019290916200008891600791860190620000d3565b5081516200009e906008906020850190620000d3565b50600955505060008054600160a060020a0316815260026020526040902055600a805460a060020a60ff021916905562000178565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f106200011657805160ff191683800117855562000146565b8280016001018555821562000146579182015b828111156200014657825182559160200191906001019062000129565b506200015492915062000158565b5090565b6200017591905b808211156200015457600081556001016200015f565b90565b6116aa80620001886000396000f3006080604052600436106101955763ffffffff7c010000000000000000000000000000000000000000000000000000000060003504166306fdde03811461019a5780630753c30c14610224578063095ea7b3146102475780630e136b191461026b5780630ecb93c01461029457806318160ddd146102b557806323b872dd146102dc57806326976e3f1461030657806327e235e314610337578063313ce56714610358578063353907141461036d5780633eaaf86b146103825780633f4ba83a1461039757806359bf1abe146103ac5780635c658165146103cd5780635c975abb146103f457806370a08231146104095780638456cb591461042a578063893d20e81461043f5780638da5cb5b1461045457806395d89b4114610469578063a9059cbb1461047e578063c0324c77146104a2578063cc872b66146104bd578063db006a75146104d5578063dd62ed3e146104ed578063dd644f7214610514578063e47d606014610529578063e4997dc51461054a578063e5b5019a1461056b578063f2fde38b14610580578063f3bdc228146105a1575b600080fd5b3480156101a657600080fd5b506101af6105c2565b6040805160208082528351818301528351919283929083019185019080838360005b838110156101e95781810151838201526020016101d1565b50505050905090810190601f1680156102165780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b34801561023057600080fd5b50610245600160a060020a0360043516610650565b005b34801561025357600080fd5b50610245600160a060020a03600435166024356106e8565b34801561027757600080fd5b506102806107aa565b604080519115158252519081900360200190f35b3480156102a057600080fd5b50610245600160a060020a03600435166107ba565b3480156102c157600080fd5b506102ca61082c565b60408051918252519081900360200190f35b3480156102e857600080fd5b50610245600160a060020a03600435811690602435166044356108e8565b34801561031257600080fd5b5061031b6109be565b60408051600160a060020a039092168252519081900360200190f35b34801561034357600080fd5b506102ca600160a060020a03600435166109cd565b34801561036457600080fd5b506102ca6109df565b34801561037957600080fd5b506102ca6109e5565b34801561038e57600080fd5b506102ca6109eb565b3480156103a357600080fd5b506102456109f1565b3480156103b857600080fd5b50610280600160a060020a0360043516610a67565b3480156103d957600080fd5b506102ca600160a060020a0360043581169060243516610a89565b34801561040057600080fd5b50610280610aa6565b34801561041557600080fd5b506102ca600160a060020a0360043516610ab6565b34801561043657600080fd5b50610245610b76565b34801561044b57600080fd5b5061031b610bf1565b34801561046057600080fd5b5061031b610c00565b34801561047557600080fd5b506101af610c0f565b34801561048a57600080fd5b50610245600160a060020a0360043516602435610c6a565b3480156104ae57600080fd5b50610245600435602435610d4f565b3480156104c957600080fd5b50610245600435610de4565b3480156104e157600080fd5b50610245600435610e8f565b3480156104f957600080fd5b506102ca600160a060020a0360043581169060243516610f3a565b34801561052057600080fd5b506102ca611005565b34801561053557600080fd5b50610280600160a060020a036004351661100b565b34801561055657600080fd5b50610245600160a060020a0360043516611020565b34801561057757600080fd5b506102ca61108f565b34801561058c57600080fd5b50610245600160a060020a0360043516611095565b3480156105ad57600080fd5b50610245600160a060020a03600435166110e7565b6007805460408051602060026001851615610100026000190190941693909304601f810184900484028201840190925281815292918301828280156106485780601f1061061d57610100808354040283529160200191610648565b820191906000526020600020905b81548152906001019060200180831161062b57829003601f168201915b505050505081565b600054600160a060020a0316331461066757600080fd5b600a805460a060020a74ff0000000000000000000000000000000000000000199091161773ffffffffffffffffffffffffffffffffffffffff1916600160a060020a03831690811790915560408051918252517fcc358699805e9a8b7f77b522628c7cb9abd07d9efb86b6fb616af1609036a99e916020908290030190a150565b604060443610156106f857600080fd5b600a5460a060020a900460ff161561079b57600a54604080517faee92d33000000000000000000000000000000000000000000000000000000008152336004820152600160a060020a038681166024830152604482018690529151919092169163aee92d3391606480830192600092919082900301818387803b15801561077e57600080fd5b505af1158015610792573d6000803e3d6000fd5b505050506107a5565b6107a58383611193565b505050565b600a5460a060020a900460ff1681565b600054600160a060020a031633146107d157600080fd5b600160a060020a038116600081815260066020908152604091829020805460ff19166001179055815192835290517f42e160154868087d6bfdc0ca23d96a1c1cfa32f1b72ba9ba27b69b98a0d819dc9281900390910190a150565b600a5460009060a060020a900460ff16156108e057600a60009054906101000a9004600160a060020a0316600160a060020a03166318160ddd6040518163ffffffff167c0100000000000000000000000000000000000000000000000000000000028152600401602060405180830381600087803b1580156108ad57600080fd5b505af11580156108c1573d6000803e3d6000fd5b505050506040513d60208110156108d757600080fd5b505190506108e5565b506001545b90565b60005460a060020a900460ff16156108ff57600080fd5b600160a060020a03831660009081526006602052604090205460ff161561092557600080fd5b600a5460a060020a900460ff16156109b357600a54604080517f8b477adb000000000000000000000000000000000000000000000000000000008152336004820152600160a060020a03868116602483015285811660448301526064820185905291519190921691638b477adb91608480830192600092919082900301818387803b15801561077e57600080fd5b6107a5838383611241565b600a54600160a060020a031681565b60026020526000908152604090205481565b60095481565b60045481565b60015481565b600054600160a060020a03163314610a0857600080fd5b60005460a060020a900460ff161515610a2057600080fd5b6000805474ff0000000000000000000000000000000000000000191681556040517f7805862f689e2f13df9f062ff482ad3ad112aca9e0847911ed832e158c525b339190a1565b600160a060020a03811660009081526006602052604090205460ff165b919050565b600560209081526000928352604080842090915290825290205481565b60005460a060020a900460ff1681565b600a5460009060a060020a900460ff1615610b6657600a54604080517f70a08231000000000000000000000000000000000000000000000000000000008152600160a060020a038581166004830152915191909216916370a082319160248083019260209291908290030181600087803b158015610b3357600080fd5b505af1158015610b47573d6000803e3d6000fd5b505050506040513d6020811015610b5d57600080fd5b50519050610a84565b610b6f8261143d565b9050610a84565b600054600160a060020a03163314610b8d57600080fd5b60005460a060020a900460ff1615610ba457600080fd5b6000805474ff0000000000000000000000000000000000000000191660a060020a1781556040517f6985a02210a168e66602d3235cb6db0e70f92b3ba4d376a33c0f3d9434bff6259190a1565b600054600160a060020a031690565b600054600160a060020a031681565b6008805460408051602060026001851615610100026000190190941693909304601f810184900484028201840190925281815292918301828280156106485780601f1061061d57610100808354040283529160200191610648565b60005460a060020a900460ff1615610c8157600080fd5b3360009081526006602052604090205460ff1615610c9e57600080fd5b600a5460a060020a900460ff1615610d4157600a54604080517f6e18980a000000000000000000000000000000000000000000000000000000008152336004820152600160a060020a0385811660248301526044820185905291519190921691636e18980a91606480830192600092919082900301818387803b158015610d2457600080fd5b505af1158015610d38573d6000803e3d6000fd5b50505050610d4b565b610d4b8282611458565b5050565b600054600160a060020a03163314610d6657600080fd5b60148210610d7357600080fd5b60328110610d8057600080fd5b6003829055600954610d9c908290600a0a63ffffffff6115c516565b600481905560035460408051918252602082019290925281517fb044a1e409eac5c48e5af22d4af52670dd1a99059537a78b31b48c6500a6354e929181900390910190a15050565b600054600160a060020a03163314610dfb57600080fd5b60015481810111610e0b57600080fd5b60008054600160a060020a031681526002602052604090205481810111610e3157600080fd5b60008054600160a060020a03168152600260209081526040918290208054840190556001805484019055815183815291517fcb8241adb0c3fdb35b70c24ce35c5eb0c17af7431c99f827d44a445ca624176a9281900390910190a150565b600054600160a060020a03163314610ea657600080fd5b600154811115610eb557600080fd5b60008054600160a060020a0316815260026020526040902054811115610eda57600080fd5b60018054829003905560008054600160a060020a031681526002602090815260409182902080548490039055815183815291517f702d5967f45f6513a38ffc42d6ba9bf230bd40e8f53b16363c7eb4fd2deb9a449281900390910190a150565b600a5460009060a060020a900460ff1615610ff257600a54604080517fdd62ed3e000000000000000000000000000000000000000000000000000000008152600160a060020a03868116600483015285811660248301529151919092169163dd62ed3e9160448083019260209291908290030181600087803b158015610fbf57600080fd5b505af1158015610fd3573d6000803e3d6000fd5b505050506040513d6020811015610fe957600080fd5b50519050610fff565b610ffc83836115fb565b90505b92915050565b60035481565b60066020526000908152604090205460ff1681565b600054600160a060020a0316331461103757600080fd5b600160a060020a038116600081815260066020908152604091829020805460ff19169055815192835290517fd7e9ec6e6ecd65492dce6bf513cd6867560d49544421d0783ddf06e76c24470c9281900390910190a150565b60001981565b600054600160a060020a031633146110ac57600080fd5b600160a060020a038116156110e4576000805473ffffffffffffffffffffffffffffffffffffffff1916600160a060020a0383161790555b50565b60008054600160a060020a031633146110ff57600080fd5b600160a060020a03821660009081526006602052604090205460ff16151561112657600080fd5b61112f82610ab6565b600160a060020a0383166000818152600260209081526040808320929092556001805485900390558151928352820183905280519293507f61e6e66b0d6339b2980aecc6ccc0039736791f0ccde9ed512e789a7fbdd698c692918290030190a15050565b604060443610156111a357600080fd5b81158015906111d45750336000908152600560209081526040808320600160a060020a038716845290915290205415155b156111de57600080fd5b336000818152600560209081526040808320600160a060020a03881680855290835292819020869055805186815290519293927f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b925929181900390910190a3505050565b600080806060606436101561125557600080fd5b600160a060020a03871660009081526005602090815260408083203384529091529020546003549094506112a4906127109061129890889063ffffffff6115c516565b9063ffffffff61162616565b92506004548311156112b65760045492505b6000198410156112f5576112d0848663ffffffff61163d16565b600160a060020a03881660009081526005602090815260408083203384529091529020555b611305858463ffffffff61163d16565b600160a060020a038816600090815260026020526040902054909250611331908663ffffffff61163d16565b600160a060020a038089166000908152600260205260408082209390935590881681522054611366908363ffffffff61164f16565b600160a060020a0387166000908152600260205260408120919091558311156113fb5760008054600160a060020a03168152600260205260409020546113b2908463ffffffff61164f16565b60008054600160a060020a0390811682526002602090815260408084209490945591548351878152935190821693918b169260008051602061165f833981519152928290030190a35b85600160a060020a031687600160a060020a031660008051602061165f833981519152846040518082815260200191505060405180910390a350505050505050565b600160a060020a031660009081526002602052604090205490565b6000806040604436101561146b57600080fd5b611486612710611298600354876115c590919063ffffffff16565b92506004548311156114985760045492505b6114a8848463ffffffff61163d16565b336000908152600260205260409020549092506114cb908563ffffffff61163d16565b3360009081526002602052604080822092909255600160a060020a038716815220546114fd908363ffffffff61164f16565b600160a060020a0386166000908152600260205260408120919091558311156115905760008054600160a060020a0316815260026020526040902054611549908463ffffffff61164f16565b60008054600160a060020a03908116825260026020908152604080842094909455915483518781529351911692339260008051602061165f83398151915292918290030190a35b604080518381529051600160a060020a03871691339160008051602061165f8339815191529181900360200190a35050505050565b6000808315156115d857600091506115f4565b508282028284828115156115e857fe5b04146115f057fe5b8091505b5092915050565b600160a060020a03918216600090815260056020908152604080832093909416825291909152205490565b600080828481151561163457fe5b04949350505050565b60008282111561164957fe5b50900390565b6000828201838110156115f057fe00ddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3efa165627a7a723058200a2a80d8b94d9b6b83d4f23236efda269f67f0f4f985bc7ff87d430ae61938700029";
        public TetherTokenDeploymentBase() : base(BYTECODE) { }
        public TetherTokenDeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("uint256", "_initialSupply", 1)]
        public virtual BigInteger InitialSupply { get; set; }
        [Parameter("string", "_name", 2)]
        public virtual string Name { get; set; }
        [Parameter("string", "_symbol", 3)]
        public virtual string Symbol { get; set; }
        [Parameter("uint256", "_decimals", 4)]
        public virtual BigInteger Decimals { get; set; }
    }

    public partial class NameFunction : NameFunctionBase { }

    [Function("name", "string")]
    public class NameFunctionBase : FunctionMessage
    {

    }

    public partial class DeprecateFunction : DeprecateFunctionBase { }

    [Function("deprecate")]
    public class DeprecateFunctionBase : FunctionMessage
    {
        [Parameter("address", "_upgradedAddress", 1)]
        public virtual string UpgradedAddress { get; set; }
    }

    public partial class ApproveFunction : ApproveFunctionBase { }

    [Function("approve")]
    public class ApproveFunctionBase : FunctionMessage
    {
        [Parameter("address", "_spender", 1)]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "_value", 2)]
        public virtual BigInteger Value { get; set; }
    }

    public partial class DeprecatedFunction : DeprecatedFunctionBase { }

    [Function("deprecated", "bool")]
    public class DeprecatedFunctionBase : FunctionMessage
    {

    }

    public partial class AddBlackListFunction : AddBlackListFunctionBase { }

    [Function("addBlackList")]
    public class AddBlackListFunctionBase : FunctionMessage
    {
        [Parameter("address", "_evilUser", 1)]
        public virtual string EvilUser { get; set; }
    }

    public partial class TotalSupplyFunction : TotalSupplyFunctionBase { }

    [Function("totalSupply", "uint256")]
    public class TotalSupplyFunctionBase : FunctionMessage
    {

    }

    public partial class TransferFromFunction : TransferFromFunctionBase { }

    [Function("transferFrom")]
    public class TransferFromFunctionBase : FunctionMessage
    {
        [Parameter("address", "_from", 1)]
        public virtual string From { get; set; }
        [Parameter("address", "_to", 2)]
        public virtual string To { get; set; }
        [Parameter("uint256", "_value", 3)]
        public virtual BigInteger Value { get; set; }
    }

    public partial class UpgradedAddressFunction : UpgradedAddressFunctionBase { }

    [Function("upgradedAddress", "address")]
    public class UpgradedAddressFunctionBase : FunctionMessage
    {

    }

    public partial class BalancesFunction : BalancesFunctionBase { }

    [Function("balances", "uint256")]
    public class BalancesFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class DecimalsFunction : DecimalsFunctionBase { }

    [Function("decimals", "uint256")]
    public class DecimalsFunctionBase : FunctionMessage
    {

    }

    public partial class MaximumFeeFunction : MaximumFeeFunctionBase { }

    [Function("maximumFee", "uint256")]
    public class MaximumFeeFunctionBase : FunctionMessage
    {

    }

    public partial class UnpauseFunction : UnpauseFunctionBase { }

    [Function("unpause")]
    public class UnpauseFunctionBase : FunctionMessage
    {

    }

    public partial class GetBlackListStatusFunction : GetBlackListStatusFunctionBase { }

    [Function("getBlackListStatus", "bool")]
    public class GetBlackListStatusFunctionBase : FunctionMessage
    {
        [Parameter("address", "_maker", 1)]
        public virtual string Maker { get; set; }
    }

    public partial class AllowedFunction : AllowedFunctionBase { }

    [Function("allowed", "uint256")]
    public class AllowedFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
        [Parameter("address", "", 2)]
        public virtual string ReturnValue2 { get; set; }
    }

    public partial class PausedFunction : PausedFunctionBase { }

    [Function("paused", "bool")]
    public class PausedFunctionBase : FunctionMessage
    {

    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "who", 1)]
        public virtual string Who { get; set; }
    }

    public partial class PauseFunction : PauseFunctionBase { }

    [Function("pause")]
    public class PauseFunctionBase : FunctionMessage
    {

    }

    public partial class GetOwnerFunction : GetOwnerFunctionBase { }

    [Function("getOwner", "address")]
    public class GetOwnerFunctionBase : FunctionMessage
    {

    }

    public partial class OwnerFunction : OwnerFunctionBase { }

    [Function("owner", "address")]
    public class OwnerFunctionBase : FunctionMessage
    {

    }

    public partial class SymbolFunction : SymbolFunctionBase { }

    [Function("symbol", "string")]
    public class SymbolFunctionBase : FunctionMessage
    {

    }

    public partial class TransferFunction : TransferFunctionBase { }

    [Function("transfer")]
    public class TransferFunctionBase : FunctionMessage
    {
        [Parameter("address", "_to", 1)]
        public virtual string To { get; set; }
        [Parameter("uint256", "_value", 2)]
        public virtual BigInteger Value { get; set; }
    }

    public partial class SetParamsFunction : SetParamsFunctionBase { }

    [Function("setParams")]
    public class SetParamsFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "newBasisPoints", 1)]
        public virtual BigInteger NewBasisPoints { get; set; }
        [Parameter("uint256", "newMaxFee", 2)]
        public virtual BigInteger NewMaxFee { get; set; }
    }

    public partial class IssueFunction : IssueFunctionBase { }

    [Function("issue")]
    public class IssueFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amount", 1)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class RedeemFunction : RedeemFunctionBase { }

    [Function("redeem")]
    public class RedeemFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amount", 1)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class AllowanceFunction : AllowanceFunctionBase { }

    [Function("allowance", "uint256")]
    public class AllowanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public virtual string Owner { get; set; }
        [Parameter("address", "_spender", 2)]
        public virtual string Spender { get; set; }
    }

    public partial class BasisPointsRateFunction : BasisPointsRateFunctionBase { }

    [Function("basisPointsRate", "uint256")]
    public class BasisPointsRateFunctionBase : FunctionMessage
    {

    }

    public partial class IsBlackListedFunction : IsBlackListedFunctionBase { }

    [Function("isBlackListed", "bool")]
    public class IsBlackListedFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class RemoveBlackListFunction : RemoveBlackListFunctionBase { }

    [Function("removeBlackList")]
    public class RemoveBlackListFunctionBase : FunctionMessage
    {
        [Parameter("address", "_clearedUser", 1)]
        public virtual string ClearedUser { get; set; }
    }

    public partial class MAX_UINTFunction : MAX_UINTFunctionBase { }

    [Function("MAX_UINT", "uint256")]
    public class MAX_UINTFunctionBase : FunctionMessage
    {

    }

    public partial class TransferOwnershipFunction : TransferOwnershipFunctionBase { }

    [Function("transferOwnership")]
    public class TransferOwnershipFunctionBase : FunctionMessage
    {
        [Parameter("address", "newOwner", 1)]
        public virtual string NewOwner { get; set; }
    }

    public partial class DestroyBlackFundsFunction : DestroyBlackFundsFunctionBase { }

    [Function("destroyBlackFunds")]
    public class DestroyBlackFundsFunctionBase : FunctionMessage
    {
        [Parameter("address", "_blackListedUser", 1)]
        public virtual string BlackListedUser { get; set; }
    }

    public partial class IssueEventDTO : IssueEventDTOBase { }

    [Event("Issue")]
    public class IssueEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "amount", 1, false )]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class RedeemEventDTO : RedeemEventDTOBase { }

    [Event("Redeem")]
    public class RedeemEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "amount", 1, false )]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class DeprecateEventDTO : DeprecateEventDTOBase { }

    [Event("Deprecate")]
    public class DeprecateEventDTOBase : IEventDTO
    {
        [Parameter("address", "newAddress", 1, false )]
        public virtual string NewAddress { get; set; }
    }

    public partial class ParamsEventDTO : ParamsEventDTOBase { }

    [Event("Params")]
    public class ParamsEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "feeBasisPoints", 1, false )]
        public virtual BigInteger FeeBasisPoints { get; set; }
        [Parameter("uint256", "maxFee", 2, false )]
        public virtual BigInteger MaxFee { get; set; }
    }

    public partial class DestroyedBlackFundsEventDTO : DestroyedBlackFundsEventDTOBase { }

    [Event("DestroyedBlackFunds")]
    public class DestroyedBlackFundsEventDTOBase : IEventDTO
    {
        [Parameter("address", "_blackListedUser", 1, false )]
        public virtual string BlackListedUser { get; set; }
        [Parameter("uint256", "_balance", 2, false )]
        public virtual BigInteger Balance { get; set; }
    }

    public partial class AddedBlackListEventDTO : AddedBlackListEventDTOBase { }

    [Event("AddedBlackList")]
    public class AddedBlackListEventDTOBase : IEventDTO
    {
        [Parameter("address", "_user", 1, false )]
        public virtual string User { get; set; }
    }

    public partial class RemovedBlackListEventDTO : RemovedBlackListEventDTOBase { }

    [Event("RemovedBlackList")]
    public class RemovedBlackListEventDTOBase : IEventDTO
    {
        [Parameter("address", "_user", 1, false )]
        public virtual string User { get; set; }
    }

    public partial class ApprovalEventDTO : ApprovalEventDTOBase { }

    [Event("Approval")]
    public class ApprovalEventDTOBase : IEventDTO
    {
        [Parameter("address", "owner", 1, true )]
        public virtual string Owner { get; set; }
        [Parameter("address", "spender", 2, true )]
        public virtual string Spender { get; set; }
        [Parameter("uint256", "value", 3, false )]
        public virtual BigInteger Value { get; set; }
    }

    public partial class TransferEventDTO : TransferEventDTOBase { }

    [Event("Transfer")]
    public class TransferEventDTOBase : IEventDTO
    {
        [Parameter("address", "from", 1, true )]
        public virtual string From { get; set; }
        [Parameter("address", "to", 2, true )]
        public virtual string To { get; set; }
        [Parameter("uint256", "value", 3, false )]
        public virtual BigInteger Value { get; set; }
    }





    public partial class NameOutputDTO : NameOutputDTOBase { }

    [FunctionOutput]
    public class NameOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }





    public partial class DeprecatedOutputDTO : DeprecatedOutputDTOBase { }

    [FunctionOutput]
    public class DeprecatedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }



    public partial class TotalSupplyOutputDTO : TotalSupplyOutputDTOBase { }

    [FunctionOutput]
    public class TotalSupplyOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class UpgradedAddressOutputDTO : UpgradedAddressOutputDTOBase { }

    [FunctionOutput]
    public class UpgradedAddressOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class BalancesOutputDTO : BalancesOutputDTOBase { }

    [FunctionOutput]
    public class BalancesOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class DecimalsOutputDTO : DecimalsOutputDTOBase { }

    [FunctionOutput]
    public class DecimalsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class MaximumFeeOutputDTO : MaximumFeeOutputDTOBase { }

    [FunctionOutput]
    public class MaximumFeeOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetBlackListStatusOutputDTO : GetBlackListStatusOutputDTOBase { }

    [FunctionOutput]
    public class GetBlackListStatusOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class AllowedOutputDTO : AllowedOutputDTOBase { }

    [FunctionOutput]
    public class AllowedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class PausedOutputDTO : PausedOutputDTOBase { }

    [FunctionOutput]
    public class PausedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class BalanceOfOutputDTO : BalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class BalanceOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class GetOwnerOutputDTO : GetOwnerOutputDTOBase { }

    [FunctionOutput]
    public class GetOwnerOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class OwnerOutputDTO : OwnerOutputDTOBase { }

    [FunctionOutput]
    public class OwnerOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class SymbolOutputDTO : SymbolOutputDTOBase { }

    [FunctionOutput]
    public class SymbolOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }









    public partial class AllowanceOutputDTO : AllowanceOutputDTOBase { }

    [FunctionOutput]
    public class AllowanceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "remaining", 1)]
        public virtual BigInteger Remaining { get; set; }
    }

    public partial class BasisPointsRateOutputDTO : BasisPointsRateOutputDTOBase { }

    [FunctionOutput]
    public class BasisPointsRateOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class IsBlackListedOutputDTO : IsBlackListedOutputDTOBase { }

    [FunctionOutput]
    public class IsBlackListedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }



    public partial class MAX_UINTOutputDTO : MAX_UINTOutputDTOBase { }

    [FunctionOutput]
    public class MAX_UINTOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }




}
