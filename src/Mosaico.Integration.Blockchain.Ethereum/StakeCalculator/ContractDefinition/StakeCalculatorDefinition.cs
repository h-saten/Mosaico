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

namespace Mosaico.Integration.Blockchain.Ethereum.StakeCalculator.ContractDefinition
{


    public partial class StakeCalculatorDeployment : StakeCalculatorDeploymentBase
    {
        public StakeCalculatorDeployment() : base(BYTECODE) { }
        public StakeCalculatorDeployment(string byteCode) : base(byteCode) { }
    }

    public class StakeCalculatorDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "608060405234801561001057600080fd5b506040516105a93803806105a983398101604081905261002f91610040565b60019290925560035560055561006e565b60008060006060848603121561005557600080fd5b8351925060208401519150604084015190509250925092565b61052c8061007d6000396000f3fe608060405234801561001057600080fd5b50600436106100625760003560e01c806306661abd1461006757806327e235e31461007d5780632d925b901461009d5780632ddbd13a146100b05780635101e128146100b857806370a08231146100cd575b600080fd5b6002545b60405190815260200160405180910390f35b61006b61008b3660046103a1565b60006020819052908152604090205481565b61006b6100ab3660046103bc565b6100f6565b60045461006b565b6100cb6100c63660046103e6565b61015a565b005b61006b6100db3660046103a1565b6001600160a01b031660009081526020819052604090205490565b60008061012f61012861010860045490565b6001600160a01b0387166000908152602081905260409020545b90610226565b8490610268565b905061013b81806102ef565b9050600354811115610151575050600354610154565b90505b92915050565b6001600160a01b0383166000908152602081905260409020541580156101805750600082115b1561019b57600280549060006101958361042f565b91905055505b60006101b86127106101226005548661026890919063ffffffff16565b905060006101d06101c98385610268565b85906102ef565b6001600160a01b0386166000908152602081905260409020549091506101f690826102ef565b6001600160a01b03861660009081526020819052604090205560045461021c90826102ef565b6004555050505050565b600061015183836040518060400160405280601a81526020017f536166654d6174683a206469766973696f6e206279207a65726f00000000000081525061034e565b60008260000361027a57506000610154565b60006102868385610448565b9050826102938583610467565b146101515760405162461bcd60e51b815260206004820152602160248201527f536166654d6174683a206d756c7469706c69636174696f6e206f766572666c6f6044820152607760f81b60648201526084015b60405180910390fd5b6000806102fc8385610489565b9050838110156101515760405162461bcd60e51b815260206004820152601b60248201527f536166654d6174683a206164646974696f6e206f766572666c6f77000000000060448201526064016102e6565b6000818361036f5760405162461bcd60e51b81526004016102e691906104a1565b50600061037c8486610467565b95945050505050565b80356001600160a01b038116811461039c57600080fd5b919050565b6000602082840312156103b357600080fd5b61015182610385565b600080604083850312156103cf57600080fd5b6103d883610385565b946020939093013593505050565b6000806000606084860312156103fb57600080fd5b61040484610385565b95602085013595506040909401359392505050565b634e487b7160e01b600052601160045260246000fd5b60006001820161044157610441610419565b5060010190565b600081600019048311821515161561046257610462610419565b500290565b60008261048457634e487b7160e01b600052601260045260246000fd5b500490565b6000821982111561049c5761049c610419565b500190565b600060208083528351808285015260005b818110156104ce578581018301518582016040015282016104b2565b818111156104e0576000604083870101525b50601f01601f191692909201604001939250505056fea2646970667358221220d35911abe880c1844968b5a89db94a54ed01382b8857008d4d2f6f7082fe976964736f6c634300080e0033";
        public StakeCalculatorDeploymentBase() : base(BYTECODE) { }
        public StakeCalculatorDeploymentBase(string byteCode) : base(byteCode) { }
        [Parameter("uint256", "reward", 1)]
        public virtual BigInteger Reward { get; set; }
        [Parameter("uint256", "maxReward", 2)]
        public virtual BigInteger MaxReward { get; set; }
        [Parameter("uint256", "calculationBonus", 3)]
        public virtual BigInteger CalculationBonus { get; set; }
    }

    public partial class AddFunction : AddFunctionBase { }

    [Function("add")]
    public class AddFunctionBase : FunctionMessage
    {
        [Parameter("address", "wallet", 1)]
        public virtual string Wallet { get; set; }
        [Parameter("uint256", "amount", 2)]
        public virtual BigInteger Amount { get; set; }
        [Parameter("uint256", "stakingDays", 3)]
        public virtual BigInteger StakingDays { get; set; }
    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "wallet", 1)]
        public virtual string Wallet { get; set; }
    }

    public partial class BalancesFunction : BalancesFunctionBase { }

    [Function("balances", "uint256")]
    public class BalancesFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class CountFunction : CountFunctionBase { }

    [Function("count", "uint256")]
    public class CountFunctionBase : FunctionMessage
    {

    }

    public partial class EstimateRewardFunction : EstimateRewardFunctionBase { }

    [Function("estimateReward", "uint256")]
    public class EstimateRewardFunctionBase : FunctionMessage
    {
        [Parameter("address", "wallet", 1)]
        public virtual string Wallet { get; set; }
        [Parameter("uint256", "totalPool", 2)]
        public virtual BigInteger TotalPool { get; set; }
    }

    public partial class TotalFunction : TotalFunctionBase { }

    [Function("total", "uint256")]
    public class TotalFunctionBase : FunctionMessage
    {

    }



    public partial class BalanceOfOutputDTO : BalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class BalanceOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class BalancesOutputDTO : BalancesOutputDTOBase { }

    [FunctionOutput]
    public class BalancesOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class CountOutputDTO : CountOutputDTOBase { }

    [FunctionOutput]
    public class CountOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class EstimateRewardOutputDTO : EstimateRewardOutputDTOBase { }

    [FunctionOutput]
    public class EstimateRewardOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class TotalOutputDTO : TotalOutputDTOBase { }

    [FunctionOutput]
    public class TotalOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }
}
