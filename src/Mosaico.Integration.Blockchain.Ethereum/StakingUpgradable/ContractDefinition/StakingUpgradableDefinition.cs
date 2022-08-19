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

namespace Mosaico.Integration.Blockchain.Ethereum.StakingUpgradable.ContractDefinition
{


    public partial class StakingUpgradableDeployment : StakingUpgradableDeploymentBase
    {
        public StakingUpgradableDeployment() : base(BYTECODE) { }
        public StakingUpgradableDeployment(string byteCode) : base(byteCode) { }
    }

    public class StakingUpgradableDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405234801561001057600080fd5b506126d1806100206000396000f3fe608060405234801561001057600080fd5b50600436106101425760003560e01c806370a08231116100b85780638da5cb5b1161007c5780638da5cb5b146102b75780639a278a8b146102c8578063a694fc3a146102d0578063b0d2d011146102e3578063f2fde38b146102eb578063fb932108146102fe57600080fd5b806370a0823114610251578063715018a61461027a57806372f702f314610282578063750142e6146102a7578063817b1cd2146102af57600080fd5b80632fc6bd871161010a5780632fc6bd87146101cc5780633ccfd60b146101df5780634043891a146101e75780635c975abb146101fa578063610f8eb0146102105780636ee010ae1461024957600080fd5b80630e9a68da1461014757806316934fc41461015c5780631e83409a1461018557806322c28011146101a657806325c33e13146101b9575b600080fd5b61015a610155366004611cde565b610311565b005b61016f61016a366004611d74565b6103f7565b60405161017c9190611d8f565b60405180910390f35b610198610193366004611d74565b6104af565b60405190815260200161017c565b61015a6101b4366004611e0d565b6106b7565b61015a6101c7366004611e0d565b6106e6565b61015a6101da366004611e0d565b610715565b61015a610744565b61015a6101f5366004611e0d565b610b30565b60655460ff16604051901515815260200161017c565b61019861021e366004611e26565b6001600160a01b03918216600090815260d56020908152604080832093909416825291909152205490565b60ca54610198565b61019861025f366004611d74565b6001600160a01b0316600090815260cf602052604090205490565b61015a610ba2565b60cb546001600160a01b03165b6040516001600160a01b03909116815260200161017c565b60d354610198565b60d254610198565b6033546001600160a01b031661028f565b60d654610198565b61015a6102de366004611e0d565b610bd8565b60cc54610198565b61015a6102f9366004611d74565b610ffc565b61019861030c366004611e59565b611097565b600061031d6001611814565b90508015610335576000805461ff0019166101001790555b61033d6118a1565b6103456118c8565b61034d6118f7565b610355611926565b815160c955602082015160ca55604082015160cb80546001600160a01b039283166001600160a01b031991821617909155606084015160cc55608084015160cd805491909316911617905560a082015160ce55606460d65580156103f3576000805461ff0019169055604051600181527f7f26b83ff96e1f2b6a682f133852f6798a09c465da95921460cefb38474024989060200160405180910390a15b5050565b6001600160a01b038116600090815260d060209081526040808320805482518185028101850190935280835260609492939192909184015b828210156104a45760008481526020908190206040805160a0810182526004860290920180546001600160a01b039081168452600180830154858701526002830154938501939093526003909101549081166060840152600160a01b900460ff1615156080830152908352909201910161042f565b505050509050919050565b60006002609754036104dc5760405162461bcd60e51b81526004016104d390611e83565b60405180910390fd5b60026097556001600160a01b0382166105275760405162461bcd60e51b815260206004820152600d60248201526c24b73b30b634b2103a37b5b2b760991b60448201526064016104d3565b6000610533833361021e565b90506000811161057b5760405162461bcd60e51b81526020600482015260136024820152724e6f7468696e6720746f20776974686472617760681b60448201526064016104d3565b6040516370a0823160e01b8152306004820152839082906001600160a01b038316906370a0823190602401602060405180830381865afa1580156105c3573d6000803e3d6000fd5b505050506040513d601f19601f820116820180604052508101906105e79190611eba565b10156106055760405162461bcd60e51b81526004016104d390611ed3565b6001600160a01b03811663a9059cbb336040516001600160e01b031960e084901b1681526001600160a01b039091166004820152602481018590526044016020604051808303816000875af1158015610662573d6000803e3d6000fd5b505050506040513d601f19601f820116820180604052508101906106869190611f30565b50506001600160a01b0392909216600090815260d56020908152604080832033845290915281205550600160975590565b6033546001600160a01b031633146106e15760405162461bcd60e51b81526004016104d390611f52565b60ca55565b6033546001600160a01b031633146107105760405162461bcd60e51b81526004016104d390611f52565b60c955565b6033546001600160a01b0316331461073f5760405162461bcd60e51b81526004016104d390611f52565b60cc55565b6002609754036107665760405162461bcd60e51b81526004016104d390611e83565b600260975560cb5460cd546001600160a01b0391821691166000805b33600090815260d060205260409020548110156107f45733600090815260d06020526040902080546107e09190839081106107bf576107bf611f87565b9060005260206000209060040201600101548361195590919063ffffffff16565b91506107ed600182611fb3565b9050610782565b506000811161083b5760405162461bcd60e51b81526020600482015260136024820152724e6f7468696e6720746f20776974686472617760681b60448201526064016104d3565b806001600160a01b03831663598af9e73360ce5460405160e084901b6001600160e01b03191681526001600160a01b0390921660048301523060248301526044820152606401602060405180830381865afa15801561089e573d6000803e3d6000fd5b505050506040513d601f19601f820116820180604052508101906108c29190611eba565b10156109105760405162461bcd60e51b815260206004820152601e60248201527f596f75206861766520746f20617070726f766520544d4f53206669727374000060448201526064016104d3565b6040516370a0823160e01b815230600482015281906001600160a01b038516906370a0823190602401602060405180830381865afa158015610956573d6000803e3d6000fd5b505050506040513d601f19601f8201168201806040525081019061097a9190611eba565b10156109985760405162461bcd60e51b81526004016104d390611ed3565b33600090815260d0602052604081206109b091611c56565b60d2546109bd90826119bb565b60d25560005b60d154811015610a6c57336001600160a01b031660d182815481106109ea576109ea611f87565b60009182526020909120600490910201546001600160a01b031603610a5a5760d18181548110610a1c57610a1c611f87565b60009182526020822060049091020180546001600160a01b031916815560018101829055600281019190915560030180546001600160a81b03191690555b610a65600182611fb3565b90506109c3565b5033600090815260cf6020526040902054610a8790826119bb565b33600081815260cf6020526040902091909155610aa490826119fd565b6001600160a01b03831663a9059cbb336040516001600160e01b031960e084901b1681526001600160a01b039091166004820152602481018490526044016020604051808303816000875af1158015610b01573d6000803e3d6000fd5b505050506040513d601f19601f82011682018060405250810190610b259190611f30565b505060016097555050565b6033546001600160a01b03163314610b5a5760405162461bcd60e51b81526004016104d390611f52565b60008111610b9d5760405162461bcd60e51b815260206004820152601060248201526f546f6f206c6974746c6520626f6e757360801b60448201526064016104d3565b60d655565b6033546001600160a01b03163314610bcc5760405162461bcd60e51b81526004016104d390611f52565b610bd66000611a87565b565b600260975403610bfa5760405162461bcd60e51b81526004016104d390611e83565b600260975560cb546001600160a01b0316818163dd62ed3e336040516001600160e01b031960e084901b1681526001600160a01b039091166004820152306024820152604401602060405180830381865afa158015610c5d573d6000803e3d6000fd5b505050506040513d601f19601f82011682018060405250810190610c819190611eba565b1015610cdf5760405162461bcd60e51b815260206004820152602760248201527f5374616b696e673a2043616e6e6f74207374616b65206d6f7265207468616e206044820152663cb7ba9037bbb760c91b60648201526084016104d3565b60cc54821015610d4c5760405162461bcd60e51b815260206004820152603260248201527f5374616b696e673a2043616e6e6f74207374616b65206c657373207468616e206044820152711d1a19481b5a5b9a5b5d5b48185b5bdd5b9d60721b60648201526084016104d3565b6001600160a01b0381166323b872dd336040516001600160e01b031960e084901b1681526001600160a01b039091166004820152306024820152604481018590526064016020604051808303816000875af1158015610daf573d6000803e3d6000fd5b505050506040513d601f19601f82011682018060405250810190610dd39190611f30565b50610dde3383611ad9565b60006040518060a00160405280610df23390565b6001600160a01b03908116825260208083018790524260408085019190915260cb5483166060808601919091526001608095860181905233600090815260d08552838120805480840182559082528582208951600492830290910180546001600160a01b0319908116928a16929092178155968a01805188860155958a0180516002890155948a018051600390980180549a8c018051998b166001600160a81b03199c8d1617600160a01b9a15158b021790915560d1805496870181559094528a517f695fb3134ad82c3b8022bc5464edd0bcc9424ef672b52245dcb6ab2374327ce395909302948501805490921692891692909217905593517f695fb3134ad82c3b8022bc5464edd0bcc9424ef672b52245dcb6ab2374327ce483015591517f695fb3134ad82c3b8022bc5464edd0bcc9424ef672b52245dcb6ab2374327ce582015591517f695fb3134ad82c3b8022bc5464edd0bcc9424ef672b52245dcb6ab2374327ce690920180549151929094169416939093179215150291909117905560d254909150610f849084611955565b60d25533600090815260cf6020526040902054610fa19084611955565b33600081815260cf6020908152604091829020939093558051868152429381019390935290917f1449c6dd7851abc30abf37f57715f492010519147cc2652fbc38202c18a6ee90910160405180910390a25050600160975550565b6033546001600160a01b031633146110265760405162461bcd60e51b81526004016104d390611f52565b6001600160a01b03811661108b5760405162461bcd60e51b815260206004820152602660248201527f4f776e61626c653a206e6577206f776e657220697320746865207a65726f206160448201526564647265737360d01b60648201526084016104d3565b61109481611a87565b50565b60006002609754036110bb5760405162461bcd60e51b81526004016104d390611e83565b60026097556001600160a01b0383166111165760405162461bcd60e51b815260206004820152601c60248201527f496e76616c69642072657761726420746f6b656e20616464726573730000000060448201526064016104d3565b60ca54611121611b1e565b101561117e5760405162461bcd60e51b815260206004820152602660248201527f5374616b696e673a20526577617264206379636c65206e6f742066696e6973686044820152651959081e595d60d21b60648201526084016104d3565b82826001600160a01b03821663dd62ed3e336040516001600160e01b031960e084901b1681526001600160a01b039091166004820152306024820152604401602060405180830381865afa1580156111da573d6000803e3d6000fd5b505050506040513d601f19601f820116820180604052508101906111fe9190611eba565b101561124c5760405162461bcd60e51b815260206004820152601b60248201527f5374616b696e673a20496e73756666696369656e742066756e6473000000000060448201526064016104d3565b60008360c96000015460d65460405161126490611c77565b92835260208301919091526040820152606001604051809103906000f080158015611293573d6000803e3d6000fd5b50905060005b60d15481101561144b5760006001600160a01b031660d182815481106112c1576112c1611f87565b60009182526020909120600490910201546001600160a01b031614801590611318575060d181815481106112f7576112f7611f87565b6000918252602090912060049091020160030154600160a01b900460ff1615155b1561143957600061134c60d1838154811061133557611335611f87565b906000526020600020906004020160020154611b56565b60ca5490915061135e90600190611fcb565b811061143757826001600160a01b0316635101e12860d1848154811061138657611386611f87565b600091825260209091206004909102015460d180546001600160a01b0390921691869081106113b7576113b7611f87565b906000526020600020906004020160010154846040518463ffffffff1660e01b8152600401611404939291906001600160a01b039390931683526020830191909152604082015260600190565b600060405180830381600087803b15801561141e57600080fd5b505af1158015611432573d6000803e3d6000fd5b505050505b505b611444600182611fb3565b9050611299565b5060005b60d15481101561165d5760006001600160a01b031660d1828154811061147757611477611f87565b60009182526020909120600490910201546001600160a01b0316148015906114ce575060d181815481106114ad576114ad611f87565b6000918252602090912060049091020160030154600160a01b900460ff1615155b1561164b576000826001600160a01b0316632d925b9060d184815481106114f7576114f7611f87565b600091825260209091206004918202015460405160e084901b6001600160e01b03191681526001600160a01b039091169181019190915260248101899052604401602060405180830381865afa158015611555573d6000803e3d6000fd5b505050506040513d601f19601f820116820180604052508101906115799190611eba565b90508015611649576115ef8160d560008a6001600160a01b03166001600160a01b03168152602001908152602001600020600060d186815481106115bf576115bf611f87565b600091825260208083206004909202909101546001600160a01b0316835282019290925260400190205490611955565b6001600160a01b038816600090815260d56020526040812060d180549192918690811061161e5761161e611f87565b600091825260208083206004909202909101546001600160a01b031683528201929092526040019020555b505b611656600182611fb3565b905061144f565b506000816001600160a01b03166306661abd6040518163ffffffff1660e01b8152600401602060405180830381865afa15801561169e573d6000803e3d6000fd5b505050506040513d601f19601f820116820180604052508101906116c29190611eba565b1115611751576001600160a01b0382166323b872dd336040516001600160e01b031960e084901b1681526001600160a01b039091166004820152306024820152604481018790526064016020604051808303816000875af115801561172b573d6000803e3d6000fd5b505050506040513d601f19601f8201168201806040525081019061174f9190611f30565b505b60d35461175e9085611955565b60d3819055504260d4819055507f97791d3ac1343e05805a2f905fa80b249c2ca58cf9fef455d4fa7ec13ce5832184826001600160a01b03166306661abd6040518163ffffffff1660e01b8152600401602060405180830381865afa1580156117cb573d6000803e3d6000fd5b505050506040513d601f19601f820116820180604052508101906117ef9190611eba565b6040805192835260208301919091520160405180910390a15050600160975550919050565b60008054610100900460ff161561185b578160ff1660011480156118375750303b155b6118535760405162461bcd60e51b81526004016104d390611fe2565b506000919050565b60005460ff8084169116106118825760405162461bcd60e51b81526004016104d390611fe2565b506000805460ff191660ff92909216919091179055600190565b919050565b600054610100900460ff16610bd65760405162461bcd60e51b81526004016104d390612030565b600054610100900460ff166118ef5760405162461bcd60e51b81526004016104d390612030565b610bd6611b8b565b600054610100900460ff1661191e5760405162461bcd60e51b81526004016104d390612030565b610bd6611bbb565b600054610100900460ff1661194d5760405162461bcd60e51b81526004016104d390612030565b610bd6611bee565b6000806119628385611fb3565b9050838110156119b45760405162461bcd60e51b815260206004820152601b60248201527f536166654d6174683a206164646974696f6e206f766572666c6f77000000000060448201526064016104d3565b9392505050565b60006119b483836040518060400160405280601e81526020017f536166654d6174683a207375627472616374696f6e206f766572666c6f770000815250611c1c565b60cd5460ce54604051637a94c56560e11b81526001600160a01b038581166004830152602482019290925260448101849052911690819063f5298aca906064015b6020604051808303816000875af1158015611a5d573d6000803e3d6000fd5b505050506040513d601f19601f82011682018060405250810190611a819190611f30565b50505050565b603380546001600160a01b038381166001600160a01b0319831681179093556040519116919082907f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e090600090a35050565b60cd5460ce54604051630ab714fb60e11b81526001600160a01b038581166004830152602482019290925260448101849052911690819063156e29f690606401611a3e565b60006018603c8060d45442611b339190611fcb565b611b3d919061207b565b611b47919061207b565b611b51919061207b565b905090565b60006018603c80611b678542611fcb565b611b71919061207b565b611b7b919061207b565b611b85919061207b565b92915050565b600054610100900460ff16611bb25760405162461bcd60e51b81526004016104d390612030565b610bd633611a87565b600054610100900460ff16611be25760405162461bcd60e51b81526004016104d390612030565b6065805460ff19169055565b600054610100900460ff16611c155760405162461bcd60e51b81526004016104d390612030565b6001609755565b60008184841115611c405760405162461bcd60e51b81526004016104d3919061209d565b506000611c4d8486611fcb565b95945050505050565b50805460008255600402906000526020600020908101906110949190611c84565b6105a9806120f383390190565b5b80821115611cc35780546001600160a01b031916815560006001820181905560028201556003810180546001600160a81b0319169055600401611c85565b5090565b80356001600160a01b038116811461189c57600080fd5b600060c08284031215611cf057600080fd5b60405160c0810181811067ffffffffffffffff82111715611d2157634e487b7160e01b600052604160045260246000fd5b80604052508235815260208301356020820152611d4060408401611cc7565b604082015260608301356060820152611d5b60808401611cc7565b608082015260a083013560a08201528091505092915050565b600060208284031215611d8657600080fd5b6119b482611cc7565b602080825282518282018190526000919060409081850190868401855b82811015611e0057815180516001600160a01b039081168652878201518887015286820151878701526060808301519091169086015260809081015115159085015260a09093019290850190600101611dac565b5091979650505050505050565b600060208284031215611e1f57600080fd5b5035919050565b60008060408385031215611e3957600080fd5b611e4283611cc7565b9150611e5060208401611cc7565b90509250929050565b60008060408385031215611e6c57600080fd5b611e7583611cc7565b946020939093013593505050565b6020808252601f908201527f5265656e7472616e637947756172643a207265656e7472616e742063616c6c00604082015260600190565b600060208284031215611ecc57600080fd5b5051919050565b6020808252603c908201527f496e73756666696369656e742066756e6473206f6e20736d61727420636f6e7460408201527f726163742e20506c656173652074727920616761696e206c6174657200000000606082015260800190565b600060208284031215611f4257600080fd5b815180151581146119b457600080fd5b6020808252818101527f4f776e61626c653a2063616c6c6572206973206e6f7420746865206f776e6572604082015260600190565b634e487b7160e01b600052603260045260246000fd5b634e487b7160e01b600052601160045260246000fd5b60008219821115611fc657611fc6611f9d565b500190565b600082821015611fdd57611fdd611f9d565b500390565b6020808252602e908201527f496e697469616c697a61626c653a20636f6e747261637420697320616c72656160408201526d191e481a5b9a5d1a585b1a5e995960921b606082015260800190565b6020808252602b908201527f496e697469616c697a61626c653a20636f6e7472616374206973206e6f74206960408201526a6e697469616c697a696e6760a81b606082015260800190565b60008261209857634e487b7160e01b600052601260045260246000fd5b500490565b600060208083528351808285015260005b818110156120ca578581018301518582016040015282016120ae565b818111156120dc576000604083870101525b50601f01601f191692909201604001939250505056fe608060405234801561001057600080fd5b506040516105a93803806105a983398101604081905261002f91610040565b60019290925560035560055561006e565b60008060006060848603121561005557600080fd5b8351925060208401519150604084015190509250925092565b61052c8061007d6000396000f3fe608060405234801561001057600080fd5b50600436106100625760003560e01c806306661abd1461006757806327e235e31461007d5780632d925b901461009d5780632ddbd13a146100b05780635101e128146100b857806370a08231146100cd575b600080fd5b6002545b60405190815260200160405180910390f35b61006b61008b3660046103a1565b60006020819052908152604090205481565b61006b6100ab3660046103bc565b6100f6565b60045461006b565b6100cb6100c63660046103e6565b61015a565b005b61006b6100db3660046103a1565b6001600160a01b031660009081526020819052604090205490565b60008061012f61012861010860045490565b6001600160a01b0387166000908152602081905260409020545b90610226565b8490610268565b905061013b81806102ef565b9050600354811115610151575050600354610154565b90505b92915050565b6001600160a01b0383166000908152602081905260409020541580156101805750600082115b1561019b57600280549060006101958361042f565b91905055505b60006101b86127106101226005548661026890919063ffffffff16565b905060006101d06101c98385610268565b85906102ef565b6001600160a01b0386166000908152602081905260409020549091506101f690826102ef565b6001600160a01b03861660009081526020819052604090205560045461021c90826102ef565b6004555050505050565b600061015183836040518060400160405280601a81526020017f536166654d6174683a206469766973696f6e206279207a65726f00000000000081525061034e565b60008260000361027a57506000610154565b60006102868385610448565b9050826102938583610467565b146101515760405162461bcd60e51b815260206004820152602160248201527f536166654d6174683a206d756c7469706c69636174696f6e206f766572666c6f6044820152607760f81b60648201526084015b60405180910390fd5b6000806102fc8385610489565b9050838110156101515760405162461bcd60e51b815260206004820152601b60248201527f536166654d6174683a206164646974696f6e206f766572666c6f77000000000060448201526064016102e6565b6000818361036f5760405162461bcd60e51b81526004016102e691906104a1565b50600061037c8486610467565b95945050505050565b80356001600160a01b038116811461039c57600080fd5b919050565b6000602082840312156103b357600080fd5b61015182610385565b600080604083850312156103cf57600080fd5b6103d883610385565b946020939093013593505050565b6000806000606084860312156103fb57600080fd5b61040484610385565b95602085013595506040909401359392505050565b634e487b7160e01b600052601160045260246000fd5b60006001820161044157610441610419565b5060010190565b600081600019048311821515161561046257610462610419565b500290565b60008261048457634e487b7160e01b600052601260045260246000fd5b500490565b6000821982111561049c5761049c610419565b500190565b600060208083528351808285015260005b818110156104ce578581018301518582016040015282016104b2565b818111156104e0576000604083870101525b50601f01601f191692909201604001939250505056fea2646970667358221220d35911abe880c1844968b5a89db94a54ed01382b8857008d4d2f6f7082fe976964736f6c634300080e0033a26469706673582212201e85932d28d0cc7ac567e483f8e993a4393f655a5920114b3d9124b92f399c2a64736f6c634300080e0033";
        public StakingUpgradableDeploymentBase() : base(BYTECODE) { }
        public StakingUpgradableDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "_staker", 1)]
        public virtual string Staker { get; set; }
    }

    public partial class CalculationBonusFunction : CalculationBonusFunctionBase { }

    [Function("calculationBonus", "uint256")]
    public class CalculationBonusFunctionBase : FunctionMessage
    {

    }

    public partial class ClaimFunction : ClaimFunctionBase { }

    [Function("claim", "uint256")]
    public class ClaimFunctionBase : FunctionMessage
    {
        [Parameter("address", "tokenAddress", 1)]
        public virtual string TokenAddress { get; set; }
    }

    public partial class ClaimableBalanceOfFunction : ClaimableBalanceOfFunctionBase { }

    [Function("claimableBalanceOf", "uint256")]
    public class ClaimableBalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "token", 1)]
        public virtual string Token { get; set; }
        [Parameter("address", "wallet", 2)]
        public virtual string Wallet { get; set; }
    }

    public partial class DistributeFunction : DistributeFunctionBase { }

    [Function("distribute", "uint256")]
    public class DistributeFunctionBase : FunctionMessage
    {
        [Parameter("address", "rewardTokenAddress", 1)]
        public virtual string RewardTokenAddress { get; set; }
        [Parameter("uint256", "amount", 2)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class InitializeFunction : InitializeFunctionBase { }

    [Function("initialize")]
    public class InitializeFunctionBase : FunctionMessage
    {
        [Parameter("tuple", "settings_", 1)]
        public virtual StakingSettings Settings { get; set; }
    }

    public partial class MinimumStakingAmountFunction : MinimumStakingAmountFunctionBase { }

    [Function("minimumStakingAmount", "uint256")]
    public class MinimumStakingAmountFunctionBase : FunctionMessage
    {

    }

    public partial class OwnerFunction : OwnerFunctionBase { }

    [Function("owner", "address")]
    public class OwnerFunctionBase : FunctionMessage
    {

    }

    public partial class PausedFunction : PausedFunctionBase { }

    [Function("paused", "bool")]
    public class PausedFunctionBase : FunctionMessage
    {

    }

    public partial class RenounceOwnershipFunction : RenounceOwnershipFunctionBase { }

    [Function("renounceOwnership")]
    public class RenounceOwnershipFunctionBase : FunctionMessage
    {

    }

    public partial class RewardCycleFunction : RewardCycleFunctionBase { }

    [Function("rewardCycle", "uint256")]
    public class RewardCycleFunctionBase : FunctionMessage
    {

    }

    public partial class SetCalculationBonusFunction : SetCalculationBonusFunctionBase { }

    [Function("setCalculationBonus")]
    public class SetCalculationBonusFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "bonus", 1)]
        public virtual BigInteger Bonus { get; set; }
    }

    public partial class SetMaxRewardFunction : SetMaxRewardFunctionBase { }

    [Function("setMaxReward")]
    public class SetMaxRewardFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "reward", 1)]
        public virtual BigInteger Reward { get; set; }
    }

    public partial class SetMinimumStakingAmountFunction : SetMinimumStakingAmountFunctionBase { }

    [Function("setMinimumStakingAmount")]
    public class SetMinimumStakingAmountFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "amount", 1)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class SetRewardCycleFunction : SetRewardCycleFunctionBase { }

    [Function("setRewardCycle")]
    public class SetRewardCycleFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "cycle", 1)]
        public virtual BigInteger Cycle { get; set; }
    }

    public partial class StakeFunction : StakeFunctionBase { }

    [Function("stake")]
    public class StakeFunctionBase : FunctionMessage
    {
        [Parameter("uint256", "_amount", 1)]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class StakesFunction : StakesFunctionBase { }

    [Function("stakes", typeof(StakesOutputDTO))]
    public class StakesFunctionBase : FunctionMessage
    {
        [Parameter("address", "wallet", 1)]
        public virtual string Wallet { get; set; }
    }

    public partial class StakingTokenFunction : StakingTokenFunctionBase { }

    [Function("stakingToken", "address")]
    public class StakingTokenFunctionBase : FunctionMessage
    {

    }

    public partial class TotalRewardFunction : TotalRewardFunctionBase { }

    [Function("totalReward", "uint256")]
    public class TotalRewardFunctionBase : FunctionMessage
    {

    }

    public partial class TotalStakedFunction : TotalStakedFunctionBase { }

    [Function("totalStaked", "uint256")]
    public class TotalStakedFunctionBase : FunctionMessage
    {

    }

    public partial class TransferOwnershipFunction : TransferOwnershipFunctionBase { }

    [Function("transferOwnership")]
    public class TransferOwnershipFunctionBase : FunctionMessage
    {
        [Parameter("address", "newOwner", 1)]
        public virtual string NewOwner { get; set; }
    }

    public partial class WithdrawFunction : WithdrawFunctionBase { }

    [Function("withdraw")]
    public class WithdrawFunctionBase : FunctionMessage
    {

    }

    public partial class DistributedEventDTO : DistributedEventDTOBase { }

    [Event("Distributed")]
    public class DistributedEventDTOBase : IEventDTO
    {
        [Parameter("uint256", "amount", 1, false )]
        public virtual BigInteger Amount { get; set; }
        [Parameter("uint256", "stakeholderCount", 2, false )]
        public virtual BigInteger StakeholderCount { get; set; }
    }

    public partial class InitializedEventDTO : InitializedEventDTOBase { }

    [Event("Initialized")]
    public class InitializedEventDTOBase : IEventDTO
    {
        [Parameter("uint8", "version", 1, false )]
        public virtual byte Version { get; set; }
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

    public partial class PausedEventDTO : PausedEventDTOBase { }

    [Event("Paused")]
    public class PausedEventDTOBase : IEventDTO
    {
        [Parameter("address", "account", 1, false )]
        public virtual string Account { get; set; }
    }

    public partial class StakedEventDTO : StakedEventDTOBase { }

    [Event("Staked")]
    public class StakedEventDTOBase : IEventDTO
    {
        [Parameter("address", "staker", 1, true )]
        public virtual string Staker { get; set; }
        [Parameter("uint256", "amount", 2, false )]
        public virtual BigInteger Amount { get; set; }
        [Parameter("uint256", "timestamp", 3, false )]
        public virtual BigInteger Timestamp { get; set; }
    }

    public partial class UnpausedEventDTO : UnpausedEventDTOBase { }

    [Event("Unpaused")]
    public class UnpausedEventDTOBase : IEventDTO
    {
        [Parameter("address", "account", 1, false )]
        public virtual string Account { get; set; }
    }

    public partial class BalanceOfOutputDTO : BalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class BalanceOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class CalculationBonusOutputDTO : CalculationBonusOutputDTOBase { }

    [FunctionOutput]
    public class CalculationBonusOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class ClaimableBalanceOfOutputDTO : ClaimableBalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class ClaimableBalanceOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }





    public partial class MinimumStakingAmountOutputDTO : MinimumStakingAmountOutputDTOBase { }

    [FunctionOutput]
    public class MinimumStakingAmountOutputDTOBase : IFunctionOutputDTO 
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

    public partial class PausedOutputDTO : PausedOutputDTOBase { }

    [FunctionOutput]
    public class PausedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }



    public partial class RewardCycleOutputDTO : RewardCycleOutputDTOBase { }

    [FunctionOutput]
    public class RewardCycleOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }











    public partial class StakesOutputDTO : StakesOutputDTOBase { }

    [FunctionOutput]
    public class StakesOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("tuple[]", "", 1)]
        public virtual List<Stake> ReturnValue1 { get; set; }
    }

    public partial class StakingTokenOutputDTO : StakingTokenOutputDTOBase { }

    [FunctionOutput]
    public class StakingTokenOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class TotalRewardOutputDTO : TotalRewardOutputDTOBase { }

    [FunctionOutput]
    public class TotalRewardOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class TotalStakedOutputDTO : TotalStakedOutputDTOBase { }

    [FunctionOutput]
    public class TotalStakedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }




}
