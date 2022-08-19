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
using Mosaico.Integration.Blockchain.Ethereum.StakingUpgradable.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.StakingUpgradable
{
    public partial class StakingUpgradableService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, StakingUpgradableDeployment stakingUpgradableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<StakingUpgradableDeployment>().SendRequestAndWaitForReceiptAsync(stakingUpgradableDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, StakingUpgradableDeployment stakingUpgradableDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<StakingUpgradableDeployment>().SendRequestAsync(stakingUpgradableDeployment);
        }

        public static async Task<StakingUpgradableService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, StakingUpgradableDeployment stakingUpgradableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, stakingUpgradableDeployment, cancellationTokenSource);
            return new StakingUpgradableService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public StakingUpgradableService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<BigInteger> BalanceOfQueryAsync(BalanceOfFunction balanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        
        public Task<BigInteger> BalanceOfQueryAsync(string staker, BlockParameter blockParameter = null)
        {
            var balanceOfFunction = new BalanceOfFunction();
                balanceOfFunction.Staker = staker;
            
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        public Task<BigInteger> CalculationBonusQueryAsync(CalculationBonusFunction calculationBonusFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CalculationBonusFunction, BigInteger>(calculationBonusFunction, blockParameter);
        }

        
        public Task<BigInteger> CalculationBonusQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CalculationBonusFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> ClaimRequestAsync(ClaimFunction claimFunction)
        {
             return ContractHandler.SendRequestAsync(claimFunction);
        }

        public Task<TransactionReceipt> ClaimRequestAndWaitForReceiptAsync(ClaimFunction claimFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimFunction, cancellationToken);
        }

        public Task<string> ClaimRequestAsync(string tokenAddress)
        {
            var claimFunction = new ClaimFunction();
                claimFunction.TokenAddress = tokenAddress;
            
             return ContractHandler.SendRequestAsync(claimFunction);
        }

        public Task<TransactionReceipt> ClaimRequestAndWaitForReceiptAsync(string tokenAddress, CancellationTokenSource cancellationToken = null)
        {
            var claimFunction = new ClaimFunction();
                claimFunction.TokenAddress = tokenAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(claimFunction, cancellationToken);
        }

        public Task<BigInteger> ClaimableBalanceOfQueryAsync(ClaimableBalanceOfFunction claimableBalanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ClaimableBalanceOfFunction, BigInteger>(claimableBalanceOfFunction, blockParameter);
        }

        
        public Task<BigInteger> ClaimableBalanceOfQueryAsync(string token, string wallet, BlockParameter blockParameter = null)
        {
            var claimableBalanceOfFunction = new ClaimableBalanceOfFunction();
                claimableBalanceOfFunction.Token = token;
                claimableBalanceOfFunction.Wallet = wallet;
            
            return ContractHandler.QueryAsync<ClaimableBalanceOfFunction, BigInteger>(claimableBalanceOfFunction, blockParameter);
        }

        public Task<string> DistributeRequestAsync(DistributeFunction distributeFunction)
        {
             return ContractHandler.SendRequestAsync(distributeFunction);
        }

        public Task<TransactionReceipt> DistributeRequestAndWaitForReceiptAsync(DistributeFunction distributeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(distributeFunction, cancellationToken);
        }

        public Task<string> DistributeRequestAsync(string rewardTokenAddress, BigInteger amount)
        {
            var distributeFunction = new DistributeFunction();
                distributeFunction.RewardTokenAddress = rewardTokenAddress;
                distributeFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(distributeFunction);
        }

        public Task<TransactionReceipt> DistributeRequestAndWaitForReceiptAsync(string rewardTokenAddress, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var distributeFunction = new DistributeFunction();
                distributeFunction.RewardTokenAddress = rewardTokenAddress;
                distributeFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(distributeFunction, cancellationToken);
        }

        public Task<string> InitializeRequestAsync(InitializeFunction initializeFunction)
        {
             return ContractHandler.SendRequestAsync(initializeFunction);
        }

        public Task<TransactionReceipt> InitializeRequestAndWaitForReceiptAsync(InitializeFunction initializeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(initializeFunction, cancellationToken);
        }

        public Task<string> InitializeRequestAsync(StakingSettings settings)
        {
            var initializeFunction = new InitializeFunction();
                initializeFunction.Settings = settings;
            
             return ContractHandler.SendRequestAsync(initializeFunction);
        }

        public Task<TransactionReceipt> InitializeRequestAndWaitForReceiptAsync(StakingSettings settings, CancellationTokenSource cancellationToken = null)
        {
            var initializeFunction = new InitializeFunction();
                initializeFunction.Settings = settings;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(initializeFunction, cancellationToken);
        }

        public Task<BigInteger> MinimumStakingAmountQueryAsync(MinimumStakingAmountFunction minimumStakingAmountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MinimumStakingAmountFunction, BigInteger>(minimumStakingAmountFunction, blockParameter);
        }

        
        public Task<BigInteger> MinimumStakingAmountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MinimumStakingAmountFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }

        
        public Task<string> OwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(null, blockParameter);
        }

        public Task<bool> PausedQueryAsync(PausedFunction pausedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PausedFunction, bool>(pausedFunction, blockParameter);
        }

        
        public Task<bool> PausedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PausedFunction, bool>(null, blockParameter);
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

        public Task<BigInteger> RewardCycleQueryAsync(RewardCycleFunction rewardCycleFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RewardCycleFunction, BigInteger>(rewardCycleFunction, blockParameter);
        }

        
        public Task<BigInteger> RewardCycleQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<RewardCycleFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> SetCalculationBonusRequestAsync(SetCalculationBonusFunction setCalculationBonusFunction)
        {
             return ContractHandler.SendRequestAsync(setCalculationBonusFunction);
        }

        public Task<TransactionReceipt> SetCalculationBonusRequestAndWaitForReceiptAsync(SetCalculationBonusFunction setCalculationBonusFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setCalculationBonusFunction, cancellationToken);
        }

        public Task<string> SetCalculationBonusRequestAsync(BigInteger bonus)
        {
            var setCalculationBonusFunction = new SetCalculationBonusFunction();
                setCalculationBonusFunction.Bonus = bonus;
            
             return ContractHandler.SendRequestAsync(setCalculationBonusFunction);
        }

        public Task<TransactionReceipt> SetCalculationBonusRequestAndWaitForReceiptAsync(BigInteger bonus, CancellationTokenSource cancellationToken = null)
        {
            var setCalculationBonusFunction = new SetCalculationBonusFunction();
                setCalculationBonusFunction.Bonus = bonus;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setCalculationBonusFunction, cancellationToken);
        }

        public Task<string> SetMaxRewardRequestAsync(SetMaxRewardFunction setMaxRewardFunction)
        {
             return ContractHandler.SendRequestAsync(setMaxRewardFunction);
        }

        public Task<TransactionReceipt> SetMaxRewardRequestAndWaitForReceiptAsync(SetMaxRewardFunction setMaxRewardFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setMaxRewardFunction, cancellationToken);
        }

        public Task<string> SetMaxRewardRequestAsync(BigInteger reward)
        {
            var setMaxRewardFunction = new SetMaxRewardFunction();
                setMaxRewardFunction.Reward = reward;
            
             return ContractHandler.SendRequestAsync(setMaxRewardFunction);
        }

        public Task<TransactionReceipt> SetMaxRewardRequestAndWaitForReceiptAsync(BigInteger reward, CancellationTokenSource cancellationToken = null)
        {
            var setMaxRewardFunction = new SetMaxRewardFunction();
                setMaxRewardFunction.Reward = reward;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setMaxRewardFunction, cancellationToken);
        }

        public Task<string> SetMinimumStakingAmountRequestAsync(SetMinimumStakingAmountFunction setMinimumStakingAmountFunction)
        {
             return ContractHandler.SendRequestAsync(setMinimumStakingAmountFunction);
        }

        public Task<TransactionReceipt> SetMinimumStakingAmountRequestAndWaitForReceiptAsync(SetMinimumStakingAmountFunction setMinimumStakingAmountFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setMinimumStakingAmountFunction, cancellationToken);
        }

        public Task<string> SetMinimumStakingAmountRequestAsync(BigInteger amount)
        {
            var setMinimumStakingAmountFunction = new SetMinimumStakingAmountFunction();
                setMinimumStakingAmountFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(setMinimumStakingAmountFunction);
        }

        public Task<TransactionReceipt> SetMinimumStakingAmountRequestAndWaitForReceiptAsync(BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var setMinimumStakingAmountFunction = new SetMinimumStakingAmountFunction();
                setMinimumStakingAmountFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setMinimumStakingAmountFunction, cancellationToken);
        }

        public Task<string> SetRewardCycleRequestAsync(SetRewardCycleFunction setRewardCycleFunction)
        {
             return ContractHandler.SendRequestAsync(setRewardCycleFunction);
        }

        public Task<TransactionReceipt> SetRewardCycleRequestAndWaitForReceiptAsync(SetRewardCycleFunction setRewardCycleFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setRewardCycleFunction, cancellationToken);
        }

        public Task<string> SetRewardCycleRequestAsync(BigInteger cycle)
        {
            var setRewardCycleFunction = new SetRewardCycleFunction();
                setRewardCycleFunction.Cycle = cycle;
            
             return ContractHandler.SendRequestAsync(setRewardCycleFunction);
        }

        public Task<TransactionReceipt> SetRewardCycleRequestAndWaitForReceiptAsync(BigInteger cycle, CancellationTokenSource cancellationToken = null)
        {
            var setRewardCycleFunction = new SetRewardCycleFunction();
                setRewardCycleFunction.Cycle = cycle;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setRewardCycleFunction, cancellationToken);
        }

        public Task<string> StakeRequestAsync(StakeFunction stakeFunction)
        {
             return ContractHandler.SendRequestAsync(stakeFunction);
        }

        public Task<TransactionReceipt> StakeRequestAndWaitForReceiptAsync(StakeFunction stakeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(stakeFunction, cancellationToken);
        }

        public Task<string> StakeRequestAsync(BigInteger amount)
        {
            var stakeFunction = new StakeFunction();
                stakeFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(stakeFunction);
        }

        public Task<TransactionReceipt> StakeRequestAndWaitForReceiptAsync(BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var stakeFunction = new StakeFunction();
                stakeFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(stakeFunction, cancellationToken);
        }

        public Task<StakesOutputDTO> StakesQueryAsync(StakesFunction stakesFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<StakesFunction, StakesOutputDTO>(stakesFunction, blockParameter);
        }

        public Task<StakesOutputDTO> StakesQueryAsync(string wallet, BlockParameter blockParameter = null)
        {
            var stakesFunction = new StakesFunction();
                stakesFunction.Wallet = wallet;
            
            return ContractHandler.QueryDeserializingToObjectAsync<StakesFunction, StakesOutputDTO>(stakesFunction, blockParameter);
        }

        public Task<string> StakingTokenQueryAsync(StakingTokenFunction stakingTokenFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StakingTokenFunction, string>(stakingTokenFunction, blockParameter);
        }

        
        public Task<string> StakingTokenQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<StakingTokenFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> TotalRewardQueryAsync(TotalRewardFunction totalRewardFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalRewardFunction, BigInteger>(totalRewardFunction, blockParameter);
        }

        
        public Task<BigInteger> TotalRewardQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalRewardFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> TotalStakedQueryAsync(TotalStakedFunction totalStakedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalStakedFunction, BigInteger>(totalStakedFunction, blockParameter);
        }

        
        public Task<BigInteger> TotalStakedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalStakedFunction, BigInteger>(null, blockParameter);
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

        public Task<string> WithdrawRequestAsync(WithdrawFunction withdrawFunction)
        {
             return ContractHandler.SendRequestAsync(withdrawFunction);
        }

        public Task<string> WithdrawRequestAsync()
        {
             return ContractHandler.SendRequestAsync<WithdrawFunction>();
        }

        public Task<TransactionReceipt> WithdrawRequestAndWaitForReceiptAsync(WithdrawFunction withdrawFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawFunction, cancellationToken);
        }

        public Task<TransactionReceipt> WithdrawRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<WithdrawFunction>(null, cancellationToken);
        }
    }
}
