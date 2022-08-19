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

namespace Mosaico.Integration.Blockchain.Ethereum.TokenManagerv1.ContractDefinition
{


    public partial class TokenManagerv1Deployment : TokenManagerv1DeploymentBase
    {
        public TokenManagerv1Deployment() : base(BYTECODE) { }
        public TokenManagerv1Deployment(string byteCode) : base(byteCode) { }
    }

    public class TokenManagerv1DeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public TokenManagerv1DeploymentBase() : base(BYTECODE) { }
        public TokenManagerv1DeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class GetTokensFunction : GetTokensFunctionBase { }

    [Function("getTokens", "address[]")]
    public class GetTokensFunctionBase : FunctionMessage
    {

    }

    public partial class GetWeightFunction : GetWeightFunctionBase { }

    [Function("getWeight", "uint256")]
    public class GetWeightFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
        [Parameter("address", "tokenAddress", 2)]
        public virtual string TokenAddress { get; set; }
    }

    public partial class IsManagedTokenFunction : IsManagedTokenFunctionBase { }

    [Function("isManagedToken", "bool")]
    public class IsManagedTokenFunctionBase : FunctionMessage
    {
        [Parameter("address", "token", 1)]
        public virtual string Token { get; set; }
    }

    public partial class IsVotingTokenFunction : IsVotingTokenFunctionBase { }

    [Function("isVotingToken", "bool")]
    public class IsVotingTokenFunctionBase : FunctionMessage
    {
        [Parameter("address", "token", 1)]
        public virtual string Token { get; set; }
    }

    public partial class TokenAddedEventDTO : TokenAddedEventDTOBase { }

    [Event("TokenAdded")]
    public class TokenAddedEventDTOBase : IEventDTO
    {
        [Parameter("address", "tokenAddress", 1, false )]
        public virtual string TokenAddress { get; set; }
        [Parameter("bool", "isVoting", 2, false )]
        public virtual bool IsVoting { get; set; }
    }

    public partial class GetTokensOutputDTO : GetTokensOutputDTOBase { }

    [FunctionOutput]
    public class GetTokensOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address[]", "", 1)]
        public virtual List<string> ReturnValue1 { get; set; }
    }

    public partial class GetWeightOutputDTO : GetWeightOutputDTOBase { }

    [FunctionOutput]
    public class GetWeightOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class IsManagedTokenOutputDTO : IsManagedTokenOutputDTOBase { }

    [FunctionOutput]
    public class IsManagedTokenOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class IsVotingTokenOutputDTO : IsVotingTokenOutputDTOBase { }

    [FunctionOutput]
    public class IsVotingTokenOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }
}
