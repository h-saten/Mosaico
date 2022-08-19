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
using Mosaico.Integration.Blockchain.Ethereum.StakeCalculator.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.StakeCalculator
{
    public partial class StakeCalculatorService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, StakeCalculatorDeployment stakeCalculatorDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<StakeCalculatorDeployment>().SendRequestAndWaitForReceiptAsync(stakeCalculatorDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, StakeCalculatorDeployment stakeCalculatorDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<StakeCalculatorDeployment>().SendRequestAsync(stakeCalculatorDeployment);
        }

        public static async Task<StakeCalculatorService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, StakeCalculatorDeployment stakeCalculatorDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, stakeCalculatorDeployment, cancellationTokenSource);
            return new StakeCalculatorService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public StakeCalculatorService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> AddRequestAsync(AddFunction addFunction)
        {
             return ContractHandler.SendRequestAsync(addFunction);
        }

        public Task<TransactionReceipt> AddRequestAndWaitForReceiptAsync(AddFunction addFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addFunction, cancellationToken);
        }

        public Task<string> AddRequestAsync(string wallet, BigInteger amount, BigInteger stakingDays)
        {
            var addFunction = new AddFunction();
                addFunction.Wallet = wallet;
                addFunction.Amount = amount;
                addFunction.StakingDays = stakingDays;
            
             return ContractHandler.SendRequestAsync(addFunction);
        }

        public Task<TransactionReceipt> AddRequestAndWaitForReceiptAsync(string wallet, BigInteger amount, BigInteger stakingDays, CancellationTokenSource cancellationToken = null)
        {
            var addFunction = new AddFunction();
                addFunction.Wallet = wallet;
                addFunction.Amount = amount;
                addFunction.StakingDays = stakingDays;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addFunction, cancellationToken);
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

        public Task<BigInteger> BalancesQueryAsync(BalancesFunction balancesFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalancesFunction, BigInteger>(balancesFunction, blockParameter);
        }

        
        public Task<BigInteger> BalancesQueryAsync(string returnValue1, BlockParameter blockParameter = null)
        {
            var balancesFunction = new BalancesFunction();
                balancesFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<BalancesFunction, BigInteger>(balancesFunction, blockParameter);
        }

        public Task<BigInteger> CountQueryAsync(CountFunction countFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CountFunction, BigInteger>(countFunction, blockParameter);
        }

        
        public Task<BigInteger> CountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CountFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> EstimateRewardQueryAsync(EstimateRewardFunction estimateRewardFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<EstimateRewardFunction, BigInteger>(estimateRewardFunction, blockParameter);
        }

        
        public Task<BigInteger> EstimateRewardQueryAsync(string wallet, BigInteger totalPool, BlockParameter blockParameter = null)
        {
            var estimateRewardFunction = new EstimateRewardFunction();
                estimateRewardFunction.Wallet = wallet;
                estimateRewardFunction.TotalPool = totalPool;
            
            return ContractHandler.QueryAsync<EstimateRewardFunction, BigInteger>(estimateRewardFunction, blockParameter);
        }

        public Task<BigInteger> TotalQueryAsync(TotalFunction totalFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalFunction, BigInteger>(totalFunction, blockParameter);
        }

        
        public Task<BigInteger> TotalQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalFunction, BigInteger>(null, blockParameter);
        }
    }
}
