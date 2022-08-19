using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Contracts;
using System.Threading;
using Mosaico.Integration.Blockchain.Ethereum.DefaultCrowdsalev1.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.DefaultCrowdsalev1
{
    public partial class DefaultCrowdsalev1Service
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, DefaultCrowdsalev1Deployment defaultCrowdsalev1Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<DefaultCrowdsalev1Deployment>().SendRequestAndWaitForReceiptAsync(defaultCrowdsalev1Deployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, DefaultCrowdsalev1Deployment defaultCrowdsalev1Deployment)
        {
            return web3.Eth.GetContractDeploymentHandler<DefaultCrowdsalev1Deployment>().SendRequestAsync(defaultCrowdsalev1Deployment);
        }

        public static async Task<DefaultCrowdsalev1Service> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, DefaultCrowdsalev1Deployment defaultCrowdsalev1Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, defaultCrowdsalev1Deployment, cancellationTokenSource);
            return new DefaultCrowdsalev1Service(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public DefaultCrowdsalev1Service(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<BigInteger> NativeCurrencyInvestmentsQueryAsync(NativeCurrencyInvestmentsFunction nativeCurrencyInvestmentsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NativeCurrencyInvestmentsFunction, BigInteger>(nativeCurrencyInvestmentsFunction, blockParameter);
        }

        
        public Task<BigInteger> NativeCurrencyInvestmentsQueryAsync(string returnValue1, BlockParameter blockParameter = null)
        {
            var nativeCurrencyInvestmentsFunction = new NativeCurrencyInvestmentsFunction();
                nativeCurrencyInvestmentsFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<NativeCurrencyInvestmentsFunction, BigInteger>(nativeCurrencyInvestmentsFunction, blockParameter);
        }

        public Task<BigInteger> RateQueryAsync(RateFunction rateFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RateFunction, BigInteger>(rateFunction, blockParameter);
        }

        
        public Task<BigInteger> RateQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RateFunction, BigInteger>(null, blockParameter);
        }

        public Task<bool> SaleEndedQueryAsync(SaleEndedFunction saleEndedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SaleEndedFunction, bool>(saleEndedFunction, blockParameter);
        }

        
        public Task<bool> SaleEndedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SaleEndedFunction, bool>(null, blockParameter);
        }

        public Task<string> TokenQueryAsync(TokenFunction tokenFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TokenFunction, string>(tokenFunction, blockParameter);
        }

        
        public Task<string> TokenQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TokenFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> TokensSoldQueryAsync(TokensSoldFunction tokensSoldFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TokensSoldFunction, BigInteger>(tokensSoldFunction, blockParameter);
        }

        
        public Task<BigInteger> TokensSoldQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TokensSoldFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> WalletQueryAsync(WalletFunction walletFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<WalletFunction, string>(walletFunction, blockParameter);
        }

        
        public Task<string> WalletQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<WalletFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> WeiRaisedQueryAsync(WeiRaisedFunction weiRaisedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<WeiRaisedFunction, BigInteger>(weiRaisedFunction, blockParameter);
        }

        
        public Task<BigInteger> WeiRaisedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<WeiRaisedFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> AddStageAdminRequestAsync(AddStageAdminFunction addStageAdminFunction)
        {
             return ContractHandler.SendRequestAsync(addStageAdminFunction);
        }

        public Task<TransactionReceipt> AddStageAdminRequestAndWaitForReceiptAsync(AddStageAdminFunction addStageAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addStageAdminFunction, cancellationToken);
        }

        public Task<string> AddStageAdminRequestAsync(string account)
        {
            var addStageAdminFunction = new AddStageAdminFunction();
                addStageAdminFunction.Account = account;
            
             return ContractHandler.SendRequestAsync(addStageAdminFunction);
        }

        public Task<TransactionReceipt> AddStageAdminRequestAndWaitForReceiptAsync(string account, CancellationTokenSource cancellationToken = null)
        {
            var addStageAdminFunction = new AddStageAdminFunction();
                addStageAdminFunction.Account = account;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addStageAdminFunction, cancellationToken);
        }

        public Task<string> AddWhitelistAdminRequestAsync(AddWhitelistAdminFunction addWhitelistAdminFunction)
        {
             return ContractHandler.SendRequestAsync(addWhitelistAdminFunction);
        }

        public Task<TransactionReceipt> AddWhitelistAdminRequestAndWaitForReceiptAsync(AddWhitelistAdminFunction addWhitelistAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addWhitelistAdminFunction, cancellationToken);
        }

        public Task<string> AddWhitelistAdminRequestAsync(string account)
        {
            var addWhitelistAdminFunction = new AddWhitelistAdminFunction();
                addWhitelistAdminFunction.Account = account;
            
             return ContractHandler.SendRequestAsync(addWhitelistAdminFunction);
        }

        public Task<TransactionReceipt> AddWhitelistAdminRequestAndWaitForReceiptAsync(string account, CancellationTokenSource cancellationToken = null)
        {
            var addWhitelistAdminFunction = new AddWhitelistAdminFunction();
                addWhitelistAdminFunction.Account = account;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addWhitelistAdminFunction, cancellationToken);
        }

        public Task<string> AddWhitelistedRequestAsync(AddWhitelistedFunction addWhitelistedFunction)
        {
             return ContractHandler.SendRequestAsync(addWhitelistedFunction);
        }

        public Task<TransactionReceipt> AddWhitelistedRequestAndWaitForReceiptAsync(AddWhitelistedFunction addWhitelistedFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addWhitelistedFunction, cancellationToken);
        }

        public Task<string> AddWhitelistedRequestAsync(List<string> accounts)
        {
            var addWhitelistedFunction = new AddWhitelistedFunction();
                addWhitelistedFunction.Accounts = accounts;
            
             return ContractHandler.SendRequestAsync(addWhitelistedFunction);
        }

        public Task<TransactionReceipt> AddWhitelistedRequestAndWaitForReceiptAsync(List<string> accounts, CancellationTokenSource cancellationToken = null)
        {
            var addWhitelistedFunction = new AddWhitelistedFunction();
                addWhitelistedFunction.Accounts = accounts;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addWhitelistedFunction, cancellationToken);
        }

        public Task<BigInteger> BalanceOfQueryAsync(BalanceOfFunction balanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        
        public Task<BigInteger> BalanceOfQueryAsync(string wallet, BlockParameter blockParameter = null)
        {
            var balanceOfFunction = new BalanceOfFunction();
                balanceOfFunction.Wallet = wallet;
            
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        public Task<string> BuyTokensRequestAsync(BuyTokensFunction buyTokensFunction)
        {
             return ContractHandler.SendRequestAsync(buyTokensFunction);
        }

        public Task<TransactionReceipt> BuyTokensRequestAndWaitForReceiptAsync(BuyTokensFunction buyTokensFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(buyTokensFunction, cancellationToken);
        }

        public Task<string> BuyTokensRequestAsync(string beneficiary)
        {
            var buyTokensFunction = new BuyTokensFunction();
                buyTokensFunction.Beneficiary = beneficiary;
            
             return ContractHandler.SendRequestAsync(buyTokensFunction);
        }

        public Task<TransactionReceipt> BuyTokensRequestAndWaitForReceiptAsync(string beneficiary, CancellationTokenSource cancellationToken = null)
        {
            var buyTokensFunction = new BuyTokensFunction();
                buyTokensFunction.Beneficiary = beneficiary;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(buyTokensFunction, cancellationToken);
        }

        public Task<BigInteger> CapQueryAsync(CapFunction capFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CapFunction, BigInteger>(capFunction, blockParameter);
        }

        
        public Task<BigInteger> CapQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CapFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> ClaimTokensRequestAsync(ClaimTokensFunction claimTokensFunction)
        {
             return ContractHandler.SendRequestAsync(claimTokensFunction);
        }

        public Task<TransactionReceipt> ClaimTokensRequestAndWaitForReceiptAsync(ClaimTokensFunction claimTokensFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimTokensFunction, cancellationToken);
        }

        public Task<string> ClaimTokensRequestAsync(string wallet)
        {
            var claimTokensFunction = new ClaimTokensFunction();
                claimTokensFunction.Wallet = wallet;
            
             return ContractHandler.SendRequestAsync(claimTokensFunction);
        }

        public Task<TransactionReceipt> ClaimTokensRequestAndWaitForReceiptAsync(string wallet, CancellationTokenSource cancellationToken = null)
        {
            var claimTokensFunction = new ClaimTokensFunction();
                claimTokensFunction.Wallet = wallet;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimTokensFunction, cancellationToken);
        }

        public Task<string> CloseSaleRequestAsync(CloseSaleFunction closeSaleFunction)
        {
             return ContractHandler.SendRequestAsync(closeSaleFunction);
        }

        public Task<string> CloseSaleRequestAsync()
        {
             return ContractHandler.SendRequestAsync<CloseSaleFunction>();
        }

        public Task<TransactionReceipt> CloseSaleRequestAndWaitForReceiptAsync(CloseSaleFunction closeSaleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(closeSaleFunction, cancellationToken);
        }

        public Task<TransactionReceipt> CloseSaleRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<CloseSaleFunction>(null, cancellationToken);
        }

        public Task<string> ExchangeTokensRequestAsync(ExchangeTokensFunction exchangeTokensFunction)
        {
             return ContractHandler.SendRequestAsync(exchangeTokensFunction);
        }

        public Task<TransactionReceipt> ExchangeTokensRequestAndWaitForReceiptAsync(ExchangeTokensFunction exchangeTokensFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(exchangeTokensFunction, cancellationToken);
        }

        public Task<string> ExchangeTokensRequestAsync(string paymentToken, BigInteger amount)
        {
            var exchangeTokensFunction = new ExchangeTokensFunction();
                exchangeTokensFunction.PaymentToken = paymentToken;
                exchangeTokensFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(exchangeTokensFunction);
        }

        public Task<TransactionReceipt> ExchangeTokensRequestAndWaitForReceiptAsync(string paymentToken, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var exchangeTokensFunction = new ExchangeTokensFunction();
                exchangeTokensFunction.PaymentToken = paymentToken;
                exchangeTokensFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(exchangeTokensFunction, cancellationToken);
        }

        public Task<BigInteger> GetRateQueryAsync(GetRateFunction getRateFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetRateFunction, BigInteger>(getRateFunction, blockParameter);
        }

        
        public Task<BigInteger> GetRateQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetRateFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> GetTokenQueryAsync(GetTokenFunction getTokenFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetTokenFunction, string>(getTokenFunction, blockParameter);
        }

        
        public Task<string> GetTokenQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetTokenFunction, string>(null, blockParameter);
        }

        public Task<string> GetWalletQueryAsync(GetWalletFunction getWalletFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetWalletFunction, string>(getWalletFunction, blockParameter);
        }

        
        public Task<string> GetWalletQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetWalletFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> GetWeiRaisedQueryAsync(GetWeiRaisedFunction getWeiRaisedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetWeiRaisedFunction, BigInteger>(getWeiRaisedFunction, blockParameter);
        }

        
        public Task<BigInteger> GetWeiRaisedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetWeiRaisedFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> HardCapQueryAsync(HardCapFunction hardCapFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<HardCapFunction, BigInteger>(hardCapFunction, blockParameter);
        }

        
        public Task<BigInteger> HardCapQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<HardCapFunction, BigInteger>(null, blockParameter);
        }

        public Task<bool> IsStageAdminQueryAsync(IsStageAdminFunction isStageAdminFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsStageAdminFunction, bool>(isStageAdminFunction, blockParameter);
        }

        
        public Task<bool> IsStageAdminQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var isStageAdminFunction = new IsStageAdminFunction();
                isStageAdminFunction.Account = account;
            
            return ContractHandler.QueryAsync<IsStageAdminFunction, bool>(isStageAdminFunction, blockParameter);
        }

        public Task<bool> IsWhitelistAdminQueryAsync(IsWhitelistAdminFunction isWhitelistAdminFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsWhitelistAdminFunction, bool>(isWhitelistAdminFunction, blockParameter);
        }

        
        public Task<bool> IsWhitelistAdminQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var isWhitelistAdminFunction = new IsWhitelistAdminFunction();
                isWhitelistAdminFunction.Account = account;
            
            return ContractHandler.QueryAsync<IsWhitelistAdminFunction, bool>(isWhitelistAdminFunction, blockParameter);
        }

        public Task<bool> IsWhitelistedQueryAsync(IsWhitelistedFunction isWhitelistedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsWhitelistedFunction, bool>(isWhitelistedFunction, blockParameter);
        }

        
        public Task<bool> IsWhitelistedQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var isWhitelistedFunction = new IsWhitelistedFunction();
                isWhitelistedFunction.Account = account;
            
            return ContractHandler.QueryAsync<IsWhitelistedFunction, bool>(isWhitelistedFunction, blockParameter);
        }

        public Task<BigInteger> MaxIndividualCapQueryAsync(MaxIndividualCapFunction maxIndividualCapFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MaxIndividualCapFunction, BigInteger>(maxIndividualCapFunction, blockParameter);
        }

        
        public Task<BigInteger> MaxIndividualCapQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MaxIndividualCapFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> MinIndividualCapQueryAsync(MinIndividualCapFunction minIndividualCapFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MinIndividualCapFunction, BigInteger>(minIndividualCapFunction, blockParameter);
        }

        
        public Task<BigInteger> MinIndividualCapQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MinIndividualCapFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> NumberOfStagesQueryAsync(NumberOfStagesFunction numberOfStagesFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NumberOfStagesFunction, BigInteger>(numberOfStagesFunction, blockParameter);
        }

        
        public Task<BigInteger> NumberOfStagesQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NumberOfStagesFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }

        
        public Task<string> OwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(null, blockParameter);
        }

        public Task<string> PauseRequestAsync(PauseFunction pauseFunction)
        {
             return ContractHandler.SendRequestAsync(pauseFunction);
        }

        public Task<string> PauseRequestAsync()
        {
             return ContractHandler.SendRequestAsync<PauseFunction>();
        }

        public Task<TransactionReceipt> PauseRequestAndWaitForReceiptAsync(PauseFunction pauseFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(pauseFunction, cancellationToken);
        }

        public Task<TransactionReceipt> PauseRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<PauseFunction>(null, cancellationToken);
        }

        public Task<bool> PausedQueryAsync(PausedFunction pausedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PausedFunction, bool>(pausedFunction, blockParameter);
        }

        
        public Task<bool> PausedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PausedFunction, bool>(null, blockParameter);
        }

        public Task<string> RefundRequestAsync(RefundFunction refundFunction)
        {
             return ContractHandler.SendRequestAsync(refundFunction);
        }

        public Task<string> RefundRequestAsync()
        {
             return ContractHandler.SendRequestAsync<RefundFunction>();
        }

        public Task<TransactionReceipt> RefundRequestAndWaitForReceiptAsync(RefundFunction refundFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(refundFunction, cancellationToken);
        }

        public Task<TransactionReceipt> RefundRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<RefundFunction>(null, cancellationToken);
        }

        public Task<BigInteger> RemainStageTokensQueryAsync(RemainStageTokensFunction remainStageTokensFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RemainStageTokensFunction, BigInteger>(remainStageTokensFunction, blockParameter);
        }

        
        public Task<BigInteger> RemainStageTokensQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RemainStageTokensFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> RemoveStageAdminRequestAsync(RemoveStageAdminFunction removeStageAdminFunction)
        {
             return ContractHandler.SendRequestAsync(removeStageAdminFunction);
        }

        public Task<TransactionReceipt> RemoveStageAdminRequestAndWaitForReceiptAsync(RemoveStageAdminFunction removeStageAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(removeStageAdminFunction, cancellationToken);
        }

        public Task<string> RemoveStageAdminRequestAsync(string account)
        {
            var removeStageAdminFunction = new RemoveStageAdminFunction();
                removeStageAdminFunction.Account = account;
            
             return ContractHandler.SendRequestAsync(removeStageAdminFunction);
        }

        public Task<TransactionReceipt> RemoveStageAdminRequestAndWaitForReceiptAsync(string account, CancellationTokenSource cancellationToken = null)
        {
            var removeStageAdminFunction = new RemoveStageAdminFunction();
                removeStageAdminFunction.Account = account;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(removeStageAdminFunction, cancellationToken);
        }

        public Task<string> RemoveWhitelistAdminRequestAsync(RemoveWhitelistAdminFunction removeWhitelistAdminFunction)
        {
             return ContractHandler.SendRequestAsync(removeWhitelistAdminFunction);
        }

        public Task<TransactionReceipt> RemoveWhitelistAdminRequestAndWaitForReceiptAsync(RemoveWhitelistAdminFunction removeWhitelistAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(removeWhitelistAdminFunction, cancellationToken);
        }

        public Task<string> RemoveWhitelistAdminRequestAsync(string account)
        {
            var removeWhitelistAdminFunction = new RemoveWhitelistAdminFunction();
                removeWhitelistAdminFunction.Account = account;
            
             return ContractHandler.SendRequestAsync(removeWhitelistAdminFunction);
        }

        public Task<TransactionReceipt> RemoveWhitelistAdminRequestAndWaitForReceiptAsync(string account, CancellationTokenSource cancellationToken = null)
        {
            var removeWhitelistAdminFunction = new RemoveWhitelistAdminFunction();
                removeWhitelistAdminFunction.Account = account;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(removeWhitelistAdminFunction, cancellationToken);
        }

        public Task<string> RemoveWhitelistedRequestAsync(RemoveWhitelistedFunction removeWhitelistedFunction)
        {
             return ContractHandler.SendRequestAsync(removeWhitelistedFunction);
        }

        public Task<TransactionReceipt> RemoveWhitelistedRequestAndWaitForReceiptAsync(RemoveWhitelistedFunction removeWhitelistedFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(removeWhitelistedFunction, cancellationToken);
        }

        public Task<string> RemoveWhitelistedRequestAsync(string account)
        {
            var removeWhitelistedFunction = new RemoveWhitelistedFunction();
                removeWhitelistedFunction.Account = account;
            
             return ContractHandler.SendRequestAsync(removeWhitelistedFunction);
        }

        public Task<TransactionReceipt> RemoveWhitelistedRequestAndWaitForReceiptAsync(string account, CancellationTokenSource cancellationToken = null)
        {
            var removeWhitelistedFunction = new RemoveWhitelistedFunction();
                removeWhitelistedFunction.Account = account;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(removeWhitelistedFunction, cancellationToken);
        }

        public Task<string> RenounceOwnershipRequestAsync(RenounceOwnershipFunction renounceOwnershipFunction)
        {
             return ContractHandler.SendRequestAsync(renounceOwnershipFunction);
        }

        public Task<string> RenounceOwnershipRequestAsync()
        {
             return ContractHandler.SendRequestAsync<RenounceOwnershipFunction>();
        }

        public Task<TransactionReceipt> RenounceOwnershipRequestAndWaitForReceiptAsync(RenounceOwnershipFunction renounceOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(renounceOwnershipFunction, cancellationToken);
        }

        public Task<TransactionReceipt> RenounceOwnershipRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<RenounceOwnershipFunction>(null, cancellationToken);
        }

        public Task<string> RenounceStageAdminRequestAsync(RenounceStageAdminFunction renounceStageAdminFunction)
        {
             return ContractHandler.SendRequestAsync(renounceStageAdminFunction);
        }

        public Task<string> RenounceStageAdminRequestAsync()
        {
             return ContractHandler.SendRequestAsync<RenounceStageAdminFunction>();
        }

        public Task<TransactionReceipt> RenounceStageAdminRequestAndWaitForReceiptAsync(RenounceStageAdminFunction renounceStageAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(renounceStageAdminFunction, cancellationToken);
        }

        public Task<TransactionReceipt> RenounceStageAdminRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<RenounceStageAdminFunction>(null, cancellationToken);
        }

        public Task<string> RenounceWhitelistAdminRequestAsync(RenounceWhitelistAdminFunction renounceWhitelistAdminFunction)
        {
             return ContractHandler.SendRequestAsync(renounceWhitelistAdminFunction);
        }

        public Task<string> RenounceWhitelistAdminRequestAsync()
        {
             return ContractHandler.SendRequestAsync<RenounceWhitelistAdminFunction>();
        }

        public Task<TransactionReceipt> RenounceWhitelistAdminRequestAndWaitForReceiptAsync(RenounceWhitelistAdminFunction renounceWhitelistAdminFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(renounceWhitelistAdminFunction, cancellationToken);
        }

        public Task<TransactionReceipt> RenounceWhitelistAdminRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<RenounceWhitelistAdminFunction>(null, cancellationToken);
        }

        public Task<string> RenounceWhitelistedRequestAsync(RenounceWhitelistedFunction renounceWhitelistedFunction)
        {
             return ContractHandler.SendRequestAsync(renounceWhitelistedFunction);
        }

        public Task<string> RenounceWhitelistedRequestAsync()
        {
             return ContractHandler.SendRequestAsync<RenounceWhitelistedFunction>();
        }

        public Task<TransactionReceipt> RenounceWhitelistedRequestAndWaitForReceiptAsync(RenounceWhitelistedFunction renounceWhitelistedFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(renounceWhitelistedFunction, cancellationToken);
        }

        public Task<TransactionReceipt> RenounceWhitelistedRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<RenounceWhitelistedFunction>(null, cancellationToken);
        }

        public Task<BigInteger> SoftCapQueryAsync(SoftCapFunction softCapFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SoftCapFunction, BigInteger>(softCapFunction, blockParameter);
        }

        
        public Task<BigInteger> SoftCapQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SoftCapFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> StableCoinRateQueryAsync(StableCoinRateFunction stableCoinRateFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StableCoinRateFunction, BigInteger>(stableCoinRateFunction, blockParameter);
        }

        
        public Task<BigInteger> StableCoinRateQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StableCoinRateFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> StageQueryAsync(StageFunction stageFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StageFunction, string>(stageFunction, blockParameter);
        }

        
        public Task<string> StageQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StageFunction, string>(null, blockParameter);
        }

        public Task<string> StartNextStageRequestAsync(StartNextStageFunction startNextStageFunction)
        {
             return ContractHandler.SendRequestAsync(startNextStageFunction);
        }

        public Task<TransactionReceipt> StartNextStageRequestAndWaitForReceiptAsync(StartNextStageFunction startNextStageFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startNextStageFunction, cancellationToken);
        }

        public Task<string> StartNextStageRequestAsync(StageSettings settings, BigInteger stableCoinRate_)
        {
            var startNextStageFunction = new StartNextStageFunction();
                startNextStageFunction.Settings = settings;
                startNextStageFunction.StableCoinRate_ = stableCoinRate_;
            
             return ContractHandler.SendRequestAsync(startNextStageFunction);
        }

        public Task<TransactionReceipt> StartNextStageRequestAndWaitForReceiptAsync(StageSettings settings, BigInteger stableCoinRate_, CancellationTokenSource cancellationToken = null)
        {
            var startNextStageFunction = new StartNextStageFunction();
                startNextStageFunction.Settings = settings;
                startNextStageFunction.StableCoinRate_ = stableCoinRate_;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(startNextStageFunction, cancellationToken);
        }

        public Task<string> TokensDistributionRequestAsync(TokensDistributionFunction tokensDistributionFunction)
        {
             return ContractHandler.SendRequestAsync(tokensDistributionFunction);
        }

        public Task<string> TokensDistributionRequestAsync()
        {
             return ContractHandler.SendRequestAsync<TokensDistributionFunction>();
        }

        public Task<TransactionReceipt> TokensDistributionRequestAndWaitForReceiptAsync(TokensDistributionFunction tokensDistributionFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(tokensDistributionFunction, cancellationToken);
        }

        public Task<TransactionReceipt> TokensDistributionRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<TokensDistributionFunction>(null, cancellationToken);
        }

        public Task<string> TransferOwnershipRequestAsync(TransferOwnershipFunction transferOwnershipFunction)
        {
             return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public Task<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(TransferOwnershipFunction transferOwnershipFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public Task<string> TransferOwnershipRequestAsync(string newOwner)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
                transferOwnershipFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAsync(transferOwnershipFunction);
        }

        public Task<TransactionReceipt> TransferOwnershipRequestAndWaitForReceiptAsync(string newOwner, CancellationTokenSource cancellationToken = null)
        {
            var transferOwnershipFunction = new TransferOwnershipFunction();
                transferOwnershipFunction.NewOwner = newOwner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferOwnershipFunction, cancellationToken);
        }

        public Task<string> WithdrawFundsRequestAsync(WithdrawFundsFunction withdrawFundsFunction)
        {
             return ContractHandler.SendRequestAsync(withdrawFundsFunction);
        }

        public Task<string> WithdrawFundsRequestAsync()
        {
             return ContractHandler.SendRequestAsync<WithdrawFundsFunction>();
        }

        public Task<TransactionReceipt> WithdrawFundsRequestAndWaitForReceiptAsync(WithdrawFundsFunction withdrawFundsFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawFundsFunction, cancellationToken);
        }

        public Task<TransactionReceipt> WithdrawFundsRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<WithdrawFundsFunction>(null, cancellationToken);
        }
    }
}
