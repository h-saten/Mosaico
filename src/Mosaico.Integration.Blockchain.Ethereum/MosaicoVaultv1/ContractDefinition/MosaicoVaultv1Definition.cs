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

namespace Mosaico.Integration.Blockchain.Ethereum.MosaicoVaultv1.ContractDefinition
{


    public partial class MosaicoVaultv1Deployment : MosaicoVaultv1DeploymentBase
    {
        public MosaicoVaultv1Deployment() : base(BYTECODE) { }
        public MosaicoVaultv1Deployment(string byteCode) : base(byteCode) { }
    }

    public class MosaicoVaultv1DeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405234801561001057600080fd5b503361001b81610021565b50610119565b6001600160a01b03811660009081526020819052604090205460ff161561008e5760405162461bcd60e51b815260206004820152601e60248201527f4d756c74694f776e61626c653a20616c726561647920616e206f776e65720000604482015260640160405180910390fd5b6001600160a01b038116600081815260208190526040808220805460ff19166001908117909155805480820182559083527fb10e2d527612073b26eecdfd717e6a320cf44b4afac2b0732d9fcbe2b7fa0cf60180546001600160a01b03191684179055517f994a936646fe87ffe4f1e469d3d6aa417d6b855598397f323de5b449f765f0c39190a250565b61153b806101286000396000f3fe608060405234801561001057600080fd5b50600436106101005760003560e01c806381b34f1511610097578063a0e67e2b11610066578063a0e67e2b1461037e578063aa9ce2e614610393578063b9e7df1c146103a6578063bb941cff146103d157600080fd5b806381b34f151461020957806386f65a221461021c578063947087761461022f578063992924a61461035357600080fd5b80634506e935116100d35780634506e935146101945780636a8003301461019d57806371546b81146101d657806372fada5c146101f657600080fd5b806320e8c565146101055780632f54bf6e1461012b578063315a095d1461016c5780633fd97b9b14610181575b600080fd5b61011861011336600461121c565b610477565b6040519081526020015b60405180910390f35b61015c610139366004611262565b6001600160a01b031660009081526020819052604090205460ff16151560011490565b6040519015158152602001610122565b61017f61017a36600461127f565b610687565b005b61011861018f366004611298565b6108fd565b61011860065481565b6101186101ab3660046112c4565b6001600160a01b03918216600090815260026020908152604080832093909416825291909152205490565b6101e96101e4366004611262565b61092e565b60405161012291906112fd565b61017f61020436600461127f565b61099a565b61017f610217366004611341565b610aea565b6101e961022a366004611262565b610deb565b6102f161023d36600461127f565b6040805160e081018252600080825260208201819052918101829052606081018290526080810182905260a0810182905260c081019190915250600090815260056020818152604092839020835160e08101855281546001600160a01b0390811682526001830154811693820193909352600282015490921693820193909352600383015460608201526004830154608082015291015460ff808216151560a084015261010090910416151560c082015290565b6040805182516001600160a01b039081168252602080850151821690830152838301511691810191909152606080830151908201526080808301519082015260a08083015115159082015260c09182015115159181019190915260e001610122565b61036661036136600461127f565b610e55565b6040516001600160a01b039091168152602001610122565b610386610e7f565b6040516101229190611379565b6101186103a1366004611298565b610ee1565b6101186103b43660046112c4565b600260209081526000928352604080842090915290825290205481565b61042f6103df36600461127f565b60056020819052600091825260409091208054600182015460028301546003840154600485015494909501546001600160a01b039384169592841694919093169260ff8082169161010090041687565b604080516001600160a01b0398891681529688166020880152949096169385019390935260608401919091526080830152151560a082015290151560c082015260e001610122565b604051636eb1769f60e11b815233600482015230602482015260009083906001600160a01b0387169063dd62ed3e90604401602060405180830381865afa1580156104c6573d6000803e3d6000fd5b505050506040513d601f19601f820116820180604052508101906104ea91906113ba565b10156105355760405162461bcd60e51b8152602060048201526015602482015274417070726f766520746f6b656e732066697273742160581b60448201526064015b60405180910390fd5b61054a6001600160a01b038616333086610efd565b6001600160a01b03851660009081526002602090815260408083203384529091529020546105789084610f6e565b6001600160a01b0386166000908152600260209081526040808320338452909152812091909155600680549091906105af906113e9565b9182905550600081815260056020819052604090912080546001600160a01b03808a166001600160a01b0319928316178355600183018054918a1691909216179055600381018690556004810185905501805461ffff191661010017905590506106163390565b600082815260056020908152604080832060020180546001600160a01b039586166001600160a01b0319909116179055888416835260038252808320805460018181018355918552838520018690559388168352600482528220805493840181558252902001819055949350505050565b6000818152600560205260409020600401544210156106e35760405162461bcd60e51b8152602060048201526018602482015277546f6b656e7320617265207374696c6c206c6f636b65642160401b604482015260640161052c565b6000818152600560205260409020600101546001600160a01b0316336001600160a01b0316146107555760405162461bcd60e51b815260206004820152601b60248201527f596f7520617265206e6f74207468652077697468647261776572210000000000604482015260640161052c565b60008181526005602081905260409091200154610100900460ff166107bc5760405162461bcd60e51b815260206004820152601d60248201527f546f6b656e7320617265206e6f7420796574206465706f736974656421000000604482015260640161052c565b6000818152600560208190526040909120015460ff16156107ef5760405162461bcd60e51b815260040161052c90611402565b6000818152600560208181526040808420928301805460ff19166001179055600383015483546001600160a01b0390811686526002808552838720950154168552929091529091205461084191610f81565b600082815260056020908152604080832080546001600160a01b03908116855260028085528386209201541684529091529020557f9da6493a92039daf47d1f2d7a782299c5994c6323eb1e972f69c432089ec52bf81335b6000848152600560209081526040918290206003015482519485526001600160a01b039093169084015282015260600160405180910390a16108fa336000838152600560205260409020600381015490546001600160a01b03169190610f8d565b50565b6003602052816000526040600020818154811061091957600080fd5b90600052602060002001600091509150505481565b6001600160a01b03811660009081526004602090815260409182902080548351818402810184019094528084526060939283018282801561098e57602002820191906000526020600020905b81548152602001906001019080831161097a575b50505050509050919050565b6000818152600560208190526040909120015460ff16156109cd5760405162461bcd60e51b815260040161052c90611402565b6000818152600560205260409020600201546001600160a01b03163314610a2d5760405162461bcd60e51b8152602060048201526014602482015273596f7520617265206e6f742063726561746f722160601b604482015260640161052c565b6000818152600560208181526040808420928301805460ff19166001179055600383015492546001600160a01b0316845260029091528220610a9092610a703390565b6001600160a01b0316815260208101919091526040016000205490610f81565b6000828152600560209081526040808320546001600160a01b0316835260028252808320338085529252909120919091557feb8b5b87fbea5b732b8f4fd82b65d9afef713c799d98da83090f22c45577998d908290610899565b600083815260056020526040902060040154421015610b465760405162461bcd60e51b8152602060048201526018602482015277546f6b656e7320617265207374696c6c206c6f636b65642160401b604482015260640161052c565b6000838152600560205260409020600101546001600160a01b0316336001600160a01b031614610bb85760405162461bcd60e51b815260206004820152601b60248201527f596f7520617265206e6f74207468652077697468647261776572210000000000604482015260640161052c565b60008381526005602081905260409091200154610100900460ff16610c1f5760405162461bcd60e51b815260206004820152601d60248201527f546f6b656e7320617265206e6f7420796574206465706f736974656421000000604482015260640161052c565b6000838152600560208190526040909120015460ff1615610c525760405162461bcd60e51b815260040161052c90611402565b600083815260056020526040902060030154811115610cb35760405162461bcd60e51b815260206004820152601960248201527f4e6f7420656e6f75676820746f6b656e7320746f2073656e6400000000000000604482015260640161052c565b600083815260056020908152604080832080546001600160a01b0390811685526002808552838620920154168452909152902054610cf19082610f81565b600084815260056020818152604080842080546001600160a01b039081168652600280855283872090830154909116865283529084209490945591869052905260030154610d3f9082610f81565b6000848152600560205260409020600301819055610d7557600083815260056020819052604090912001805460ff191660011790555b604080518481523360208201526001600160a01b038416818301526060810183905290517f5348907260f668ccf372e72edea3ca31e8321b0317151038312c09b05d910a899181900360800190a1600083815260056020526040902054610de6906001600160a01b03168383610f8d565b505050565b6001600160a01b03811660009081526003602090815260409182902080548351818402810184019094528084526060939283018282801561098e576020028201919060005260206000209081548152602001906001019080831161097a5750505050509050919050565b60018181548110610e6557600080fd5b6000918252602090912001546001600160a01b0316905081565b60606001805480602002602001604051908101604052809291908181526020018280548015610ed757602002820191906000526020600020905b81546001600160a01b03168152600190910190602001808311610eb9575b5050505050905090565b6004602052816000526040600020818154811061091957600080fd5b6040516001600160a01b0380851660248301528316604482015260648101829052610f689085906323b872dd60e01b906084015b60408051601f198184030181529190526020810180516001600160e01b03166001600160e01b031990931692909217909152610fbd565b50505050565b6000610f7a8284611439565b9392505050565b6000610f7a8284611451565b6040516001600160a01b038316602482015260448101829052610de690849063a9059cbb60e01b90606401610f31565b6000611012826040518060400160405280602081526020017f5361666545524332303a206c6f772d6c6576656c2063616c6c206661696c6564815250856001600160a01b031661108f9092919063ffffffff16565b805190915015610de657808060200190518101906110309190611468565b610de65760405162461bcd60e51b815260206004820152602a60248201527f5361666545524332303a204552433230206f7065726174696f6e20646964206e6044820152691bdd081cdd58d8d9595960b21b606482015260840161052c565b606061109e84846000856110a6565b949350505050565b6060824710156111075760405162461bcd60e51b815260206004820152602660248201527f416464726573733a20696e73756666696369656e742062616c616e636520666f6044820152651c8818d85b1b60d21b606482015260840161052c565b843b6111555760405162461bcd60e51b815260206004820152601d60248201527f416464726573733a2063616c6c20746f206e6f6e2d636f6e7472616374000000604482015260640161052c565b600080866001600160a01b0316858760405161117191906114b6565b60006040518083038185875af1925050503d80600081146111ae576040519150601f19603f3d011682016040523d82523d6000602084013e6111b3565b606091505b50915091506111c38282866111ce565b979650505050505050565b606083156111dd575081610f7a565b8251156111ed5782518084602001fd5b8160405162461bcd60e51b815260040161052c91906114d2565b6001600160a01b03811681146108fa57600080fd5b6000806000806080858703121561123257600080fd5b843561123d81611207565b9350602085013561124d81611207565b93969395505050506040820135916060013590565b60006020828403121561127457600080fd5b8135610f7a81611207565b60006020828403121561129157600080fd5b5035919050565b600080604083850312156112ab57600080fd5b82356112b681611207565b946020939093013593505050565b600080604083850312156112d757600080fd5b82356112e281611207565b915060208301356112f281611207565b809150509250929050565b6020808252825182820181905260009190848201906040850190845b8181101561133557835183529284019291840191600101611319565b50909695505050505050565b60008060006060848603121561135657600080fd5b83359250602084013561136881611207565b929592945050506040919091013590565b6020808252825182820181905260009190848201906040850190845b818110156113355783516001600160a01b031683529284019291840191600101611395565b6000602082840312156113cc57600080fd5b5051919050565b634e487b7160e01b600052601160045260246000fd5b6000600182016113fb576113fb6113d3565b5060010190565b6020808252601d908201527f546f6b656e732061726520616c72656164792077697468647261776e21000000604082015260600190565b6000821982111561144c5761144c6113d3565b500190565b600082821015611463576114636113d3565b500390565b60006020828403121561147a57600080fd5b81518015158114610f7a57600080fd5b60005b838110156114a557818101518382015260200161148d565b83811115610f685750506000910152565b600082516114c881846020870161148a565b9190910192915050565b60208152600082518060208401526114f181604085016020870161148a565b601f01601f1916919091016040019291505056fea26469706673582212206008c530a4e9013215b6f9c9394de560080771130013350f3b4671b6657a81d064736f6c634300080d0033";
        public MosaicoVaultv1DeploymentBase() : base(BYTECODE) { }
        public MosaicoVaultv1DeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class OwnersFunction : OwnersFunctionBase { }

    [Function("_owners", "address")]
    public class OwnersFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class CancelDepositFunction : CancelDepositFunctionBase { }

    [Function("cancelDeposit")]
    public class CancelDepositFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_id", 1)]
        public virtual BigInteger Id { get; set; }
    }

    public partial class DepositFunction : DepositFunctionBase { }

    [Function("deposit", "uint256")]
    public class DepositFunctionBase : FunctionMessage
    {
        [Parameter("address", "_token", 1)]
        public virtual string Token { get; set; }
        [Parameter("address", "_withdrawer", 2)]
        public virtual string Withdrawer { get; set; }
        [Parameter("uint256", "_amount", 3)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("uint256", "_unlockTimestamp", 4)]
        public virtual BigInteger UnlockTimestamp { get; set; }
    }

    public partial class DepositsByTokenAddressFunction : DepositsByTokenAddressFunctionBase { }

    [Function("depositsByTokenAddress", "uint256")]
    public class DepositsByTokenAddressFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
        [Parameter("uint256", "", 2)]
        public virtual BigInteger ReturnValue2 { get; set; }
    }

    public partial class DepositsByWithdrawersFunction : DepositsByWithdrawersFunctionBase { }

    [Function("depositsByWithdrawers", "uint256")]
    public class DepositsByWithdrawersFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
        [Parameter("uint256", "", 2)]
        public virtual BigInteger ReturnValue2 { get; set; }
    }

    public partial class DepositsCountFunction : DepositsCountFunctionBase { }

    [Function("depositsCount", "uint256")]
    public class DepositsCountFunctionBase : FunctionMessage
    {

    }

    public partial class GetDepositsByTokenAddressFunction : GetDepositsByTokenAddressFunctionBase { }

    [Function("getDepositsByTokenAddress", "uint256[]")]
    public class GetDepositsByTokenAddressFunctionBase : FunctionMessage
    {
        [Parameter("address", "_id", 1)]
        public virtual string Id { get; set; }
    }

    public partial class GetDepositsByWithdrawerFunction : GetDepositsByWithdrawerFunctionBase { }

    [Function("getDepositsByWithdrawer", "uint256")]
    public class GetDepositsByWithdrawerFunctionBase : FunctionMessage
    {
        [Parameter("address", "_token", 1)]
        public virtual string Token { get; set; }
        [Parameter("address", "_withdrawer", 2)]
        public virtual string Withdrawer { get; set; }
    }

    public partial class GetOwnersFunction : GetOwnersFunctionBase { }

    [Function("getOwners", "address[]")]
    public class GetOwnersFunctionBase : FunctionMessage
    {

    }

    public partial class GetVaultByIdFunction : GetVaultByIdFunctionBase { }

    [Function("getVaultById", typeof(GetVaultByIdOutputDTO))]
    public class GetVaultByIdFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_id", 1)]
        public virtual BigInteger Id { get; set; }
    }

    public partial class GetVaultsByWithdrawerFunction : GetVaultsByWithdrawerFunctionBase { }

    [Function("getVaultsByWithdrawer", "uint256[]")]
    public class GetVaultsByWithdrawerFunctionBase : FunctionMessage
    {
        [Parameter("address", "_withdrawer", 1)]
        public virtual string Withdrawer { get; set; }
    }

    public partial class IsOwnerFunction : IsOwnerFunctionBase { }

    [Function("isOwner", "bool")]
    public class IsOwnerFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
    }

    public partial class LockedTokenFunction : LockedTokenFunctionBase { }

    [Function("lockedToken", typeof(LockedTokenOutputDTO))]
    public class LockedTokenFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class SendFunction : SendFunctionBase { }

    [Function("send")]
    public class SendFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_id", 1)]
        public virtual BigInteger Id { get; set; }
        [Parameter("address", "recipient", 2)]
        public virtual string Recipient { get; set; }
        [Parameter("uint256", "_amount", 3)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class WalletTokenBalanceFunction : WalletTokenBalanceFunctionBase { }

    [Function("walletTokenBalance", "uint256")]
    public class WalletTokenBalanceFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
        [Parameter("address", "", 2)]
        public virtual string ReturnValue2 { get; set; }
    }

    public partial class WithdrawTokensFunction : WithdrawTokensFunctionBase { }

    [Function("withdrawTokens")]
    public class WithdrawTokensFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_id", 1)]
        public virtual BigInteger Id { get; set; }
    }

    public partial class CanceledEventDTO : CanceledEventDTOBase { }

    [Event("Canceled")]
    public class CanceledEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "id", 1, false )]
        public virtual BigInteger Id { get; set; }
        [Parameter("address", "withdrawer", 2, false )]
        public virtual string Withdrawer { get; set; }
        [Parameter("uint256", "amount", 3, false )]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class OwnerAddedEventDTO : OwnerAddedEventDTOBase { }

    [Event("OwnerAdded")]
    public class OwnerAddedEventDTOBase : IEventDTO
    {
        [Parameter("address", "newOwner", 1, true )]
        public virtual string NewOwner { get; set; }
    }

    public partial class OwnerRemovedEventDTO : OwnerRemovedEventDTOBase { }

    [Event("OwnerRemoved")]
    public class OwnerRemovedEventDTOBase : IEventDTO
    {
        [Parameter("address", "oldOwner", 1, true )]
        public virtual string OldOwner { get; set; }
    }

    public partial class SentEventDTO : SentEventDTOBase { }

    [Event("Sent")]
    public class SentEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "id", 1, false )]
        public virtual BigInteger Id { get; set; }
        [Parameter("address", "withdrawer", 2, false )]
        public virtual string Withdrawer { get; set; }
        [Parameter("address", "recipient", 3, false )]
        public virtual string Recipient { get; set; }
        [Parameter("uint256", "amount", 4, false )]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class WithdrawEventDTO : WithdrawEventDTOBase { }

    [Event("Withdraw")]
    public class WithdrawEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "id", 1, false )]
        public virtual BigInteger Id { get; set; }
        [Parameter("address", "withdrawer", 2, false )]
        public virtual string Withdrawer { get; set; }
        [Parameter("uint256", "amount", 3, false )]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class OwnersOutputDTO : OwnersOutputDTOBase { }

    [FunctionOutput]
    public class OwnersOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }





    public partial class DepositsByTokenAddressOutputDTO : DepositsByTokenAddressOutputDTOBase { }

    [FunctionOutput]
    public class DepositsByTokenAddressOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class DepositsByWithdrawersOutputDTO : DepositsByWithdrawersOutputDTOBase { }

    [FunctionOutput]
    public class DepositsByWithdrawersOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class DepositsCountOutputDTO : DepositsCountOutputDTOBase { }

    [FunctionOutput]
    public class DepositsCountOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetDepositsByTokenAddressOutputDTO : GetDepositsByTokenAddressOutputDTOBase { }

    [FunctionOutput]
    public class GetDepositsByTokenAddressOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256[]", "", 1)]
        public virtual List<BigInteger> ReturnValue1 { get; set; }
    }

    public partial class GetDepositsByWithdrawerOutputDTO : GetDepositsByWithdrawerOutputDTOBase { }

    [FunctionOutput]
    public class GetDepositsByWithdrawerOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetOwnersOutputDTO : GetOwnersOutputDTOBase { }

    [FunctionOutput]
    public class GetOwnersOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address[]", "", 1)]
        public virtual List<string> ReturnValue1 { get; set; }
    }

    public partial class GetVaultByIdOutputDTO : GetVaultByIdOutputDTOBase { }

    [FunctionOutput]
    public class GetVaultByIdOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("tuple", "", 1)]
        public virtual Items ReturnValue1 { get; set; }
    }

    public partial class GetVaultsByWithdrawerOutputDTO : GetVaultsByWithdrawerOutputDTOBase { }

    [FunctionOutput]
    public class GetVaultsByWithdrawerOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256[]", "", 1)]
        public virtual List<BigInteger> ReturnValue1 { get; set; }
    }

    public partial class IsOwnerOutputDTO : IsOwnerOutputDTOBase { }

    [FunctionOutput]
    public class IsOwnerOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class LockedTokenOutputDTO : LockedTokenOutputDTOBase { }

    [FunctionOutput]
    public class LockedTokenOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "token", 1)]
        public virtual string Token { get; set; }
        [Parameter("address", "withdrawer", 2)]
        public virtual string Withdrawer { get; set; }
        [Parameter("address", "creator", 3)]
        public virtual string Creator { get; set; }
        [Parameter("uint256", "amount", 4)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("uint256", "unlockTimestamp", 5)]
        public virtual BigInteger UnlockTimestamp { get; set; }
        [Parameter("bool", "withdrawn", 6)]
        public virtual bool Withdrawn { get; set; }
        [Parameter("bool", "deposited", 7)]
        public virtual bool Deposited { get; set; }
    }



    public partial class WalletTokenBalanceOutputDTO : WalletTokenBalanceOutputDTOBase { }

    [FunctionOutput]
    public class WalletTokenBalanceOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }


}
