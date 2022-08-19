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

namespace Mosaico.Integration.Blockchain.Ethereum.CrowdsaleBase.ContractDefinition
{


    public partial class CrowdsaleBaseDeployment : CrowdsaleBaseDeploymentBase
    {
        public CrowdsaleBaseDeployment() : base(BYTECODE) { }
        public CrowdsaleBaseDeployment(string byteCode) : base(byteCode) { }
    }

    public class CrowdsaleBaseDeploymentBase : ContractDeploymentMessage
    {
        public static string BYTECODE = "";
        public CrowdsaleBaseDeploymentBase() : base(BYTECODE) { }
        public CrowdsaleBaseDeploymentBase(string byteCode) : base(byteCode) { }

    }

    public partial class NativeCurrencyInvestmentsFunction : NativeCurrencyInvestmentsFunctionBase { }

    [Function("_nativeCurrencyInvestments", "uint256")]
    public class NativeCurrencyInvestmentsFunctionBase : FunctionMessage
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class RateFunction : RateFunctionBase { }

    [Function("_rate", "uint256")]
    public class RateFunctionBase : FunctionMessage
    {

    }

    public partial class SaleEndedFunction : SaleEndedFunctionBase { }

    [Function("_saleEnded", "bool")]
    public class SaleEndedFunctionBase : FunctionMessage
    {

    }

    public partial class TokenFunction : TokenFunctionBase { }

    [Function("_token", "address")]
    public class TokenFunctionBase : FunctionMessage
    {

    }

    public partial class TokensSoldFunction : TokensSoldFunctionBase { }

    [Function("_tokensSold", "uint256")]
    public class TokensSoldFunctionBase : FunctionMessage
    {

    }

    public partial class WalletFunction : WalletFunctionBase { }

    [Function("_wallet", "address")]
    public class WalletFunctionBase : FunctionMessage
    {

    }

    public partial class WeiRaisedFunction : WeiRaisedFunctionBase { }

    [Function("_weiRaised", "uint256")]
    public class WeiRaisedFunctionBase : FunctionMessage
    {

    }

    public partial class AddStageAdminFunction : AddStageAdminFunctionBase { }

    [Function("addStageAdmin")]
    public class AddStageAdminFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
    }

    public partial class AddWhitelistAdminFunction : AddWhitelistAdminFunctionBase { }

    [Function("addWhitelistAdmin")]
    public class AddWhitelistAdminFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
    }

    public partial class AddWhitelistedFunction : AddWhitelistedFunctionBase { }

    [Function("addWhitelisted")]
    public class AddWhitelistedFunctionBase : FunctionMessage
    {
        [Parameter("address[]", "accounts", 1)]
        public virtual List<string> Accounts { get; set; }
    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "wallet", 1)]
        public virtual string Wallet { get; set; }
    }

    public partial class BuyTokensFunction : BuyTokensFunctionBase { }

    [Function("buyTokens")]
    public class BuyTokensFunctionBase : FunctionMessage
    {
        [Parameter("address", "beneficiary", 1)]
        public virtual string Beneficiary { get; set; }
    }

    public partial class CapFunction : CapFunctionBase { }

    [Function("cap", "uint256")]
    public class CapFunctionBase : FunctionMessage
    {

    }

    public partial class ClaimTokensFunction : ClaimTokensFunctionBase { }

    [Function("claimTokens")]
    public class ClaimTokensFunctionBase : FunctionMessage
    {
        [Parameter("address", "wallet", 1)]
        public virtual string Wallet { get; set; }
    }

    public partial class CloseSaleFunction : CloseSaleFunctionBase { }

    [Function("closeSale")]
    public class CloseSaleFunctionBase : FunctionMessage
    {

    }

    public partial class GetRateFunction : GetRateFunctionBase { }

    [Function("getRate", "uint256")]
    public class GetRateFunctionBase : FunctionMessage
    {

    }

    public partial class GetTokenFunction : GetTokenFunctionBase { }

    [Function("getToken", "address")]
    public class GetTokenFunctionBase : FunctionMessage
    {

    }

    public partial class GetWalletFunction : GetWalletFunctionBase { }

    [Function("getWallet", "address")]
    public class GetWalletFunctionBase : FunctionMessage
    {

    }

    public partial class GetWeiRaisedFunction : GetWeiRaisedFunctionBase { }

    [Function("getWeiRaised", "uint256")]
    public class GetWeiRaisedFunctionBase : FunctionMessage
    {

    }

    public partial class HardCapFunction : HardCapFunctionBase { }

    [Function("hardCap", "uint256")]
    public class HardCapFunctionBase : FunctionMessage
    {

    }

    public partial class IsStageAdminFunction : IsStageAdminFunctionBase { }

    [Function("isStageAdmin", "bool")]
    public class IsStageAdminFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
    }

    public partial class IsWhitelistAdminFunction : IsWhitelistAdminFunctionBase { }

    [Function("isWhitelistAdmin", "bool")]
    public class IsWhitelistAdminFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
    }

    public partial class IsWhitelistedFunction : IsWhitelistedFunctionBase { }

    [Function("isWhitelisted", "bool")]
    public class IsWhitelistedFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
    }

    public partial class MaxIndividualCapFunction : MaxIndividualCapFunctionBase { }

    [Function("maxIndividualCap", "uint256")]
    public class MaxIndividualCapFunctionBase : FunctionMessage
    {

    }

    public partial class MinIndividualCapFunction : MinIndividualCapFunctionBase { }

    [Function("minIndividualCap", "uint256")]
    public class MinIndividualCapFunctionBase : FunctionMessage
    {

    }

    public partial class NumberOfStagesFunction : NumberOfStagesFunctionBase { }

    [Function("numberOfStages", "uint256")]
    public class NumberOfStagesFunctionBase : FunctionMessage
    {

    }

    public partial class OwnerFunction : OwnerFunctionBase { }

    [Function("owner", "address")]
    public class OwnerFunctionBase : FunctionMessage
    {

    }

    public partial class PauseFunction : PauseFunctionBase { }

    [Function("pause")]
    public class PauseFunctionBase : FunctionMessage
    {

    }

    public partial class PausedFunction : PausedFunctionBase { }

    [Function("paused", "bool")]
    public class PausedFunctionBase : FunctionMessage
    {

    }

    public partial class RefundFunction : RefundFunctionBase { }

    [Function("refund")]
    public class RefundFunctionBase : FunctionMessage
    {
        [Parameter("address", "investorWallet", 1)]
        public virtual string InvestorWallet { get; set; }
    }

    public partial class RemainStageTokensFunction : RemainStageTokensFunctionBase { }

    [Function("remainStageTokens", "uint256")]
    public class RemainStageTokensFunctionBase : FunctionMessage
    {

    }

    public partial class RemoveStageAdminFunction : RemoveStageAdminFunctionBase { }

    [Function("removeStageAdmin")]
    public class RemoveStageAdminFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
    }

    public partial class RemoveWhitelistAdminFunction : RemoveWhitelistAdminFunctionBase { }

    [Function("removeWhitelistAdmin")]
    public class RemoveWhitelistAdminFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
    }

    public partial class RemoveWhitelistedFunction : RemoveWhitelistedFunctionBase { }

    [Function("removeWhitelisted")]
    public class RemoveWhitelistedFunctionBase : FunctionMessage
    {
        [Parameter("address", "account", 1)]
        public virtual string Account { get; set; }
    }

    public partial class RenounceOwnershipFunction : RenounceOwnershipFunctionBase { }

    [Function("renounceOwnership")]
    public class RenounceOwnershipFunctionBase : FunctionMessage
    {

    }

    public partial class RenounceStageAdminFunction : RenounceStageAdminFunctionBase { }

    [Function("renounceStageAdmin")]
    public class RenounceStageAdminFunctionBase : FunctionMessage
    {

    }

    public partial class RenounceWhitelistAdminFunction : RenounceWhitelistAdminFunctionBase { }

    [Function("renounceWhitelistAdmin")]
    public class RenounceWhitelistAdminFunctionBase : FunctionMessage
    {

    }

    public partial class RenounceWhitelistedFunction : RenounceWhitelistedFunctionBase { }

    [Function("renounceWhitelisted")]
    public class RenounceWhitelistedFunctionBase : FunctionMessage
    {

    }

    public partial class SoftCapFunction : SoftCapFunctionBase { }

    [Function("softCap", "uint256")]
    public class SoftCapFunctionBase : FunctionMessage
    {

    }

    public partial class StageFunction : StageFunctionBase { }

    [Function("stage", "string")]
    public class StageFunctionBase : FunctionMessage
    {

    }

    public partial class TokensDistributionFunction : TokensDistributionFunctionBase { }

    [Function("tokensDistribution")]
    public class TokensDistributionFunctionBase : FunctionMessage
    {

    }

    public partial class TransferOwnershipFunction : TransferOwnershipFunctionBase { }

    [Function("transferOwnership")]
    public class TransferOwnershipFunctionBase : FunctionMessage
    {
        [Parameter("address", "newOwner", 1)]
        public virtual string NewOwner { get; set; }
    }

    public partial class WithdrawFundsFunction : WithdrawFundsFunctionBase { }

    [Function("withdrawFunds")]
    public class WithdrawFundsFunctionBase : FunctionMessage
    {

    }



    public partial class FundsWithdrawalEventDTO : FundsWithdrawalEventDTOBase { }

    [Event("FundsWithdrawal")]
    public class FundsWithdrawalEventDTOBase : IEventDTO
    {
        [Parameter("address", "wallet", 1, true )]
        public virtual string Wallet { get; set; }
        [Parameter("address", "stablecoin", 2, true )]
        public virtual string Stablecoin { get; set; }
        [Parameter("uint256", "amount", 3, false )]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class NativeCurrencyInvestmentsReturnEventDTO : NativeCurrencyInvestmentsReturnEventDTOBase { }

    [Event("NativeCurrencyInvestmentsReturn")]
    public class NativeCurrencyInvestmentsReturnEventDTOBase : IEventDTO
    {
        [Parameter("address", "investor", 1, true )]
        public virtual string Investor { get; set; }
        [Parameter("uint256", "amount", 2, false )]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class NativeCurrencyWithdrawalEventDTO : NativeCurrencyWithdrawalEventDTOBase { }

    [Event("NativeCurrencyWithdrawal")]
    public class NativeCurrencyWithdrawalEventDTOBase : IEventDTO
    {
        [Parameter("address", "wallet", 1, true )]
        public virtual string Wallet { get; set; }
        [Parameter("uint256", "amount", 2, false )]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class NextStageEventDTO : NextStageEventDTOBase { }

    [Event("NextStage")]
    public class NextStageEventDTOBase : IEventDTO
    {
        [Parameter("tuple", "stage", 1, false )]
        public virtual StageSettings Stage { get; set; }
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

    public partial class StableCoinInvestmentsReturnEventDTO : StableCoinInvestmentsReturnEventDTOBase { }

    [Event("StableCoinInvestmentsReturn")]
    public class StableCoinInvestmentsReturnEventDTOBase : IEventDTO
    {
        [Parameter("address", "currency", 1, true )]
        public virtual string Currency { get; set; }
        [Parameter("address", "investor", 2, true )]
        public virtual string Investor { get; set; }
        [Parameter("uint256", "amount", 3, false )]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class StageAdminRoleAddedEventDTO : StageAdminRoleAddedEventDTOBase { }

    [Event("StageAdminRoleAdded")]
    public class StageAdminRoleAddedEventDTOBase : IEventDTO
    {
        [Parameter("address", "account", 1, true )]
        public virtual string Account { get; set; }
    }

    public partial class StageAdminRoleRemovedEventDTO : StageAdminRoleRemovedEventDTOBase { }

    [Event("StageAdminRoleRemoved")]
    public class StageAdminRoleRemovedEventDTOBase : IEventDTO
    {
        [Parameter("address", "account", 1, true )]
        public virtual string Account { get; set; }
    }

    public partial class TokensPurchasedEventDTO : TokensPurchasedEventDTOBase { }

    [Event("TokensPurchased")]
    public class TokensPurchasedEventDTOBase : IEventDTO
    {
        [Parameter("address", "purchaser", 1, true )]
        public virtual string Purchaser { get; set; }
        [Parameter("address", "beneficiary", 2, true )]
        public virtual string Beneficiary { get; set; }
        [Parameter("uint256", "value", 3, false )]
        public virtual BigInteger Value { get; set; }
        [Parameter("uint256", "amount", 4, false )]
        public virtual BigInteger Amount { get; set; }
    }

    public partial class UnpausedEventDTO : UnpausedEventDTOBase { }

    [Event("Unpaused")]
    public class UnpausedEventDTOBase : IEventDTO
    {
        [Parameter("address", "account", 1, false )]
        public virtual string Account { get; set; }
    }

    public partial class WhitelistAdminAddedEventDTO : WhitelistAdminAddedEventDTOBase { }

    [Event("WhitelistAdminAdded")]
    public class WhitelistAdminAddedEventDTOBase : IEventDTO
    {
        [Parameter("address", "account", 1, true )]
        public virtual string Account { get; set; }
    }

    public partial class WhitelistAdminRemovedEventDTO : WhitelistAdminRemovedEventDTOBase { }

    [Event("WhitelistAdminRemoved")]
    public class WhitelistAdminRemovedEventDTOBase : IEventDTO
    {
        [Parameter("address", "account", 1, true )]
        public virtual string Account { get; set; }
    }

    public partial class WhitelistedAddedEventDTO : WhitelistedAddedEventDTOBase { }

    [Event("WhitelistedAdded")]
    public class WhitelistedAddedEventDTOBase : IEventDTO
    {
        [Parameter("address", "account", 1, true )]
        public virtual string Account { get; set; }
    }

    public partial class WhitelistedRemovedEventDTO : WhitelistedRemovedEventDTOBase { }

    [Event("WhitelistedRemoved")]
    public class WhitelistedRemovedEventDTOBase : IEventDTO
    {
        [Parameter("address", "account", 1, true )]
        public virtual string Account { get; set; }
    }

    public partial class NativeCurrencyInvestmentsOutputDTO : NativeCurrencyInvestmentsOutputDTOBase { }

    [FunctionOutput]
    public class NativeCurrencyInvestmentsOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class RateOutputDTO : RateOutputDTOBase { }

    [FunctionOutput]
    public class RateOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class SaleEndedOutputDTO : SaleEndedOutputDTOBase { }

    [FunctionOutput]
    public class SaleEndedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class TokenOutputDTO : TokenOutputDTOBase { }

    [FunctionOutput]
    public class TokenOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class TokensSoldOutputDTO : TokensSoldOutputDTOBase { }

    [FunctionOutput]
    public class TokensSoldOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class WalletOutputDTO : WalletOutputDTOBase { }

    [FunctionOutput]
    public class WalletOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class WeiRaisedOutputDTO : WeiRaisedOutputDTOBase { }

    [FunctionOutput]
    public class WeiRaisedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }







    public partial class BalanceOfOutputDTO : BalanceOfOutputDTOBase { }

    [FunctionOutput]
    public class BalanceOfOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }



    public partial class CapOutputDTO : CapOutputDTOBase { }

    [FunctionOutput]
    public class CapOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }





    public partial class GetRateOutputDTO : GetRateOutputDTOBase { }

    [FunctionOutput]
    public class GetRateOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class GetTokenOutputDTO : GetTokenOutputDTOBase { }

    [FunctionOutput]
    public class GetTokenOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class GetWalletOutputDTO : GetWalletOutputDTOBase { }

    [FunctionOutput]
    public class GetWalletOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("address", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }

    public partial class GetWeiRaisedOutputDTO : GetWeiRaisedOutputDTOBase { }

    [FunctionOutput]
    public class GetWeiRaisedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class HardCapOutputDTO : HardCapOutputDTOBase { }

    [FunctionOutput]
    public class HardCapOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class IsStageAdminOutputDTO : IsStageAdminOutputDTOBase { }

    [FunctionOutput]
    public class IsStageAdminOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class IsWhitelistAdminOutputDTO : IsWhitelistAdminOutputDTOBase { }

    [FunctionOutput]
    public class IsWhitelistAdminOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class IsWhitelistedOutputDTO : IsWhitelistedOutputDTOBase { }

    [FunctionOutput]
    public class IsWhitelistedOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("bool", "", 1)]
        public virtual bool ReturnValue1 { get; set; }
    }

    public partial class MaxIndividualCapOutputDTO : MaxIndividualCapOutputDTOBase { }

    [FunctionOutput]
    public class MaxIndividualCapOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class MinIndividualCapOutputDTO : MinIndividualCapOutputDTOBase { }

    [FunctionOutput]
    public class MinIndividualCapOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class NumberOfStagesOutputDTO : NumberOfStagesOutputDTOBase { }

    [FunctionOutput]
    public class NumberOfStagesOutputDTOBase : IFunctionOutputDTO 
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



    public partial class RemainStageTokensOutputDTO : RemainStageTokensOutputDTOBase { }

    [FunctionOutput]
    public class RemainStageTokensOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }















    public partial class SoftCapOutputDTO : SoftCapOutputDTOBase { }

    [FunctionOutput]
    public class SoftCapOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("uint256", "", 1)]
        public virtual BigInteger ReturnValue1 { get; set; }
    }

    public partial class StageOutputDTO : StageOutputDTOBase { }

    [FunctionOutput]
    public class StageOutputDTOBase : IFunctionOutputDTO 
    {
        [Parameter("string", "", 1)]
        public virtual string ReturnValue1 { get; set; }
    }






}
