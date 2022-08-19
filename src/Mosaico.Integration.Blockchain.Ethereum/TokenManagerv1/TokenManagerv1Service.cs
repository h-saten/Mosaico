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
using Mosaico.Integration.Blockchain.Ethereum.TokenManagerv1.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.TokenManagerv1
{
    public partial class TokenManagerv1Service
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, TokenManagerv1Deployment tokenManagerv1Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<TokenManagerv1Deployment>().SendRequestAndWaitForReceiptAsync(tokenManagerv1Deployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, TokenManagerv1Deployment tokenManagerv1Deployment)
        {
            return web3.Eth.GetContractDeploymentHandler<TokenManagerv1Deployment>().SendRequestAsync(tokenManagerv1Deployment);
        }

        public static async Task<TokenManagerv1Service> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, TokenManagerv1Deployment tokenManagerv1Deployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, tokenManagerv1Deployment, cancellationTokenSource);
            return new TokenManagerv1Service(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public TokenManagerv1Service(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<List<string>> GetTokensQueryAsync(GetTokensFunction getTokensFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetTokensFunction, List<string>>(getTokensFunction, blockParameter);
        }

        
        public Task<List<string>> GetTokensQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetTokensFunction, List<string>>(null, blockParameter);
        }

        public Task<BigInteger> GetWeightQueryAsync(GetWeightFunction getWeightFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetWeightFunction, BigInteger>(getWeightFunction, blockParameter);
        }

        
        public Task<BigInteger> GetWeightQueryAsync(string account, string tokenAddress, BlockParameter blockParameter = null)
        {
            var getWeightFunction = new GetWeightFunction();
                getWeightFunction.Account = account;
                getWeightFunction.TokenAddress = tokenAddress;
            
            return ContractHandler.QueryAsync<GetWeightFunction, BigInteger>(getWeightFunction, blockParameter);
        }

        public Task<bool> IsManagedTokenQueryAsync(IsManagedTokenFunction isManagedTokenFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsManagedTokenFunction, bool>(isManagedTokenFunction, blockParameter);
        }

        
        public Task<bool> IsManagedTokenQueryAsync(string token, BlockParameter blockParameter = null)
        {
            var isManagedTokenFunction = new IsManagedTokenFunction();
                isManagedTokenFunction.Token = token;
            
            return ContractHandler.QueryAsync<IsManagedTokenFunction, bool>(isManagedTokenFunction, blockParameter);
        }

        public Task<bool> IsVotingTokenQueryAsync(IsVotingTokenFunction isVotingTokenFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsVotingTokenFunction, bool>(isVotingTokenFunction, blockParameter);
        }

        
        public Task<bool> IsVotingTokenQueryAsync(string token, BlockParameter blockParameter = null)
        {
            var isVotingTokenFunction = new IsVotingTokenFunction();
                isVotingTokenFunction.Token = token;
            
            return ContractHandler.QueryAsync<IsVotingTokenFunction, bool>(isVotingTokenFunction, blockParameter);
        }
    }
}
