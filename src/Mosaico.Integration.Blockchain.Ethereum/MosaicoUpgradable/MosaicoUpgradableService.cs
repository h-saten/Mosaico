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
using Mosaico.Integration.Blockchain.Ethereum.MosaicoUpgradable.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.MosaicoUpgradable
{
    public partial class MosaicoUpgradableService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, MosaicoUpgradableDeployment mosaicoUpgradableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<MosaicoUpgradableDeployment>().SendRequestAndWaitForReceiptAsync(mosaicoUpgradableDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, MosaicoUpgradableDeployment mosaicoUpgradableDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<MosaicoUpgradableDeployment>().SendRequestAsync(mosaicoUpgradableDeployment);
        }

        public static async Task<MosaicoUpgradableService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, MosaicoUpgradableDeployment mosaicoUpgradableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, mosaicoUpgradableDeployment, cancellationTokenSource);
            return new MosaicoUpgradableService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public MosaicoUpgradableService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<byte[]> DomainSeparatorQueryAsync(DomainSeparatorFunction domainSeparatorFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DomainSeparatorFunction, byte[]>(domainSeparatorFunction, blockParameter);
        }

        
        public Task<byte[]> DomainSeparatorQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DomainSeparatorFunction, byte[]>(null, blockParameter);
        }

        public Task<BigInteger> BountyFeeQueryAsync(BountyFeeFunction bountyFeeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BountyFeeFunction, BigInteger>(bountyFeeFunction, blockParameter);
        }

        
        public Task<BigInteger> BountyFeeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BountyFeeFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> BurnFeeQueryAsync(BurnFeeFunction burnFeeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BurnFeeFunction, BigInteger>(burnFeeFunction, blockParameter);
        }

        
        public Task<BigInteger> BurnFeeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BurnFeeFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> MaxTxAmountQueryAsync(MaxTxAmountFunction maxTxAmountFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MaxTxAmountFunction, BigInteger>(maxTxAmountFunction, blockParameter);
        }

        
        public Task<BigInteger> MaxTxAmountQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MaxTxAmountFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> VentureFeeQueryAsync(VentureFeeFunction ventureFeeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<VentureFeeFunction, BigInteger>(ventureFeeFunction, blockParameter);
        }

        
        public Task<BigInteger> VentureFeeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<VentureFeeFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> AllowanceQueryAsync(AllowanceFunction allowanceFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AllowanceFunction, BigInteger>(allowanceFunction, blockParameter);
        }

        
        public Task<BigInteger> AllowanceQueryAsync(string owner, string spender, BlockParameter blockParameter = null)
        {
            var allowanceFunction = new AllowanceFunction();
                allowanceFunction.Owner = owner;
                allowanceFunction.Spender = spender;
            
            return ContractHandler.QueryAsync<AllowanceFunction, BigInteger>(allowanceFunction, blockParameter);
        }

        public Task<string> ApproveRequestAsync(ApproveFunction approveFunction)
        {
             return ContractHandler.SendRequestAsync(approveFunction);
        }

        public Task<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(ApproveFunction approveFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public Task<string> ApproveRequestAsync(string spender, BigInteger amount)
        {
            var approveFunction = new ApproveFunction();
                approveFunction.Spender = spender;
                approveFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(approveFunction);
        }

        public Task<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(string spender, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var approveFunction = new ApproveFunction();
                approveFunction.Spender = spender;
                approveFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public Task<BigInteger> BalanceOfQueryAsync(BalanceOfFunction balanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        
        public Task<BigInteger> BalanceOfQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var balanceOfFunction = new BalanceOfFunction();
                balanceOfFunction.Account = account;
            
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        public Task<List<BigInteger>> BalanceOfBatchQueryAsync(BalanceOfBatchFunction balanceOfBatchFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfBatchFunction, List<BigInteger>>(balanceOfBatchFunction, blockParameter);
        }

        
        public Task<List<BigInteger>> BalanceOfBatchQueryAsync(List<string> accounts, List<BigInteger> ids, BlockParameter blockParameter = null)
        {
            var balanceOfBatchFunction = new BalanceOfBatchFunction();
                balanceOfBatchFunction.Accounts = accounts;
                balanceOfBatchFunction.Ids = ids;
            
            return ContractHandler.QueryAsync<BalanceOfBatchFunction, List<BigInteger>>(balanceOfBatchFunction, blockParameter);
        }

        public Task<string> BanRequestAsync(BanFunction banFunction)
        {
             return ContractHandler.SendRequestAsync(banFunction);
        }

        public Task<TransactionReceipt> BanRequestAndWaitForReceiptAsync(BanFunction banFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(banFunction, cancellationToken);
        }

        public Task<string> BanRequestAsync(string wallet)
        {
            var banFunction = new BanFunction();
                banFunction.Wallet = wallet;
            
             return ContractHandler.SendRequestAsync(banFunction);
        }

        public Task<TransactionReceipt> BanRequestAndWaitForReceiptAsync(string wallet, CancellationTokenSource cancellationToken = null)
        {
            var banFunction = new BanFunction();
                banFunction.Wallet = wallet;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(banFunction, cancellationToken);
        }

        public Task<string> BountyWalletQueryAsync(BountyWalletFunction bountyWalletFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BountyWalletFunction, string>(bountyWalletFunction, blockParameter);
        }

        
        public Task<string> BountyWalletQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BountyWalletFunction, string>(null, blockParameter);
        }

        public Task<string> BurnRequestAsync(BurnFunction burnFunction)
        {
             return ContractHandler.SendRequestAsync(burnFunction);
        }

        public Task<TransactionReceipt> BurnRequestAndWaitForReceiptAsync(BurnFunction burnFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(burnFunction, cancellationToken);
        }

        public Task<string> BurnRequestAsync(BigInteger amount)
        {
            var burnFunction = new BurnFunction();
                burnFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(burnFunction);
        }

        public Task<TransactionReceipt> BurnRequestAndWaitForReceiptAsync(BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var burnFunction = new BurnFunction();
                burnFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(burnFunction, cancellationToken);
        }

        public Task<byte> DecimalsQueryAsync(DecimalsFunction decimalsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, byte>(decimalsFunction, blockParameter);
        }

        
        public Task<byte> DecimalsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, byte>(null, blockParameter);
        }

        public Task<string> DecreaseAllowanceRequestAsync(DecreaseAllowanceFunction decreaseAllowanceFunction)
        {
             return ContractHandler.SendRequestAsync(decreaseAllowanceFunction);
        }

        public Task<TransactionReceipt> DecreaseAllowanceRequestAndWaitForReceiptAsync(DecreaseAllowanceFunction decreaseAllowanceFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(decreaseAllowanceFunction, cancellationToken);
        }

        public Task<string> DecreaseAllowanceRequestAsync(string spender, BigInteger subtractedValue)
        {
            var decreaseAllowanceFunction = new DecreaseAllowanceFunction();
                decreaseAllowanceFunction.Spender = spender;
                decreaseAllowanceFunction.SubtractedValue = subtractedValue;
            
             return ContractHandler.SendRequestAsync(decreaseAllowanceFunction);
        }

        public Task<TransactionReceipt> DecreaseAllowanceRequestAndWaitForReceiptAsync(string spender, BigInteger subtractedValue, CancellationTokenSource cancellationToken = null)
        {
            var decreaseAllowanceFunction = new DecreaseAllowanceFunction();
                decreaseAllowanceFunction.Spender = spender;
                decreaseAllowanceFunction.SubtractedValue = subtractedValue;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(decreaseAllowanceFunction, cancellationToken);
        }

        public Task<string> ExcludeFromFeeRequestAsync(ExcludeFromFeeFunction excludeFromFeeFunction)
        {
             return ContractHandler.SendRequestAsync(excludeFromFeeFunction);
        }

        public Task<TransactionReceipt> ExcludeFromFeeRequestAndWaitForReceiptAsync(ExcludeFromFeeFunction excludeFromFeeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(excludeFromFeeFunction, cancellationToken);
        }

        public Task<string> ExcludeFromFeeRequestAsync(string account)
        {
            var excludeFromFeeFunction = new ExcludeFromFeeFunction();
                excludeFromFeeFunction.Account = account;
            
             return ContractHandler.SendRequestAsync(excludeFromFeeFunction);
        }

        public Task<TransactionReceipt> ExcludeFromFeeRequestAndWaitForReceiptAsync(string account, CancellationTokenSource cancellationToken = null)
        {
            var excludeFromFeeFunction = new ExcludeFromFeeFunction();
                excludeFromFeeFunction.Account = account;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(excludeFromFeeFunction, cancellationToken);
        }

        public Task<string> FeeManagerQueryAsync(FeeManagerFunction feeManagerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<FeeManagerFunction, string>(feeManagerFunction, blockParameter);
        }

        
        public Task<string> FeeManagerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<FeeManagerFunction, string>(null, blockParameter);
        }

        public Task<string> IncludeInFeeRequestAsync(IncludeInFeeFunction includeInFeeFunction)
        {
             return ContractHandler.SendRequestAsync(includeInFeeFunction);
        }

        public Task<TransactionReceipt> IncludeInFeeRequestAndWaitForReceiptAsync(IncludeInFeeFunction includeInFeeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(includeInFeeFunction, cancellationToken);
        }

        public Task<string> IncludeInFeeRequestAsync(string account)
        {
            var includeInFeeFunction = new IncludeInFeeFunction();
                includeInFeeFunction.Account = account;
            
             return ContractHandler.SendRequestAsync(includeInFeeFunction);
        }

        public Task<TransactionReceipt> IncludeInFeeRequestAndWaitForReceiptAsync(string account, CancellationTokenSource cancellationToken = null)
        {
            var includeInFeeFunction = new IncludeInFeeFunction();
                includeInFeeFunction.Account = account;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(includeInFeeFunction, cancellationToken);
        }

        public Task<string> IncreaseAllowanceRequestAsync(IncreaseAllowanceFunction increaseAllowanceFunction)
        {
             return ContractHandler.SendRequestAsync(increaseAllowanceFunction);
        }

        public Task<TransactionReceipt> IncreaseAllowanceRequestAndWaitForReceiptAsync(IncreaseAllowanceFunction increaseAllowanceFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(increaseAllowanceFunction, cancellationToken);
        }

        public Task<string> IncreaseAllowanceRequestAsync(string spender, BigInteger addedValue)
        {
            var increaseAllowanceFunction = new IncreaseAllowanceFunction();
                increaseAllowanceFunction.Spender = spender;
                increaseAllowanceFunction.AddedValue = addedValue;
            
             return ContractHandler.SendRequestAsync(increaseAllowanceFunction);
        }

        public Task<TransactionReceipt> IncreaseAllowanceRequestAndWaitForReceiptAsync(string spender, BigInteger addedValue, CancellationTokenSource cancellationToken = null)
        {
            var increaseAllowanceFunction = new IncreaseAllowanceFunction();
                increaseAllowanceFunction.Spender = spender;
                increaseAllowanceFunction.AddedValue = addedValue;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(increaseAllowanceFunction, cancellationToken);
        }

        public Task<string> InitializeRequestAsync(InitializeFunction initializeFunction)
        {
             return ContractHandler.SendRequestAsync(initializeFunction);
        }

        public Task<TransactionReceipt> InitializeRequestAndWaitForReceiptAsync(InitializeFunction initializeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(initializeFunction, cancellationToken);
        }

        public Task<string> InitializeRequestAsync(string bountyWallet, string ventureWallet, string tokenOwner)
        {
            var initializeFunction = new InitializeFunction();
                initializeFunction.BountyWallet = bountyWallet;
                initializeFunction.VentureWallet = ventureWallet;
                initializeFunction.TokenOwner = tokenOwner;
            
             return ContractHandler.SendRequestAsync(initializeFunction);
        }

        public Task<TransactionReceipt> InitializeRequestAndWaitForReceiptAsync(string bountyWallet, string ventureWallet, string tokenOwner, CancellationTokenSource cancellationToken = null)
        {
            var initializeFunction = new InitializeFunction();
                initializeFunction.BountyWallet = bountyWallet;
                initializeFunction.VentureWallet = ventureWallet;
                initializeFunction.TokenOwner = tokenOwner;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(initializeFunction, cancellationToken);
        }

        public Task<bool> IsBannedQueryAsync(IsBannedFunction isBannedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsBannedFunction, bool>(isBannedFunction, blockParameter);
        }

        
        public Task<bool> IsBannedQueryAsync(string wallet, BlockParameter blockParameter = null)
        {
            var isBannedFunction = new IsBannedFunction();
                isBannedFunction.Wallet = wallet;
            
            return ContractHandler.QueryAsync<IsBannedFunction, bool>(isBannedFunction, blockParameter);
        }

        public Task<bool> IsExcludedFromFeeQueryAsync(IsExcludedFromFeeFunction isExcludedFromFeeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsExcludedFromFeeFunction, bool>(isExcludedFromFeeFunction, blockParameter);
        }

        
        public Task<bool> IsExcludedFromFeeQueryAsync(string account, BlockParameter blockParameter = null)
        {
            var isExcludedFromFeeFunction = new IsExcludedFromFeeFunction();
                isExcludedFromFeeFunction.Account = account;
            
            return ContractHandler.QueryAsync<IsExcludedFromFeeFunction, bool>(isExcludedFromFeeFunction, blockParameter);
        }

        public Task<string> MintRequestAsync(MintFunction mintFunction)
        {
             return ContractHandler.SendRequestAsync(mintFunction);
        }

        public Task<TransactionReceipt> MintRequestAndWaitForReceiptAsync(MintFunction mintFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintFunction, cancellationToken);
        }

        public Task<string> MintRequestAsync(BigInteger amount, string walletAddress)
        {
            var mintFunction = new MintFunction();
                mintFunction.Amount = amount;
                mintFunction.WalletAddress = walletAddress;
            
             return ContractHandler.SendRequestAsync(mintFunction);
        }

        public Task<TransactionReceipt> MintRequestAndWaitForReceiptAsync(BigInteger amount, string walletAddress, CancellationTokenSource cancellationToken = null)
        {
            var mintFunction = new MintFunction();
                mintFunction.Amount = amount;
                mintFunction.WalletAddress = walletAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(mintFunction, cancellationToken);
        }

        public Task<string> NameQueryAsync(NameFunction nameFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(nameFunction, blockParameter);
        }

        
        public Task<string> NameQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> NoncesQueryAsync(NoncesFunction noncesFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NoncesFunction, BigInteger>(noncesFunction, blockParameter);
        }

        
        public Task<BigInteger> NoncesQueryAsync(string owner, BlockParameter blockParameter = null)
        {
            var noncesFunction = new NoncesFunction();
                noncesFunction.Owner = owner;
            
            return ContractHandler.QueryAsync<NoncesFunction, BigInteger>(noncesFunction, blockParameter);
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

        public Task<string> PermitRequestAsync(PermitFunction permitFunction)
        {
             return ContractHandler.SendRequestAsync(permitFunction);
        }

        public Task<TransactionReceipt> PermitRequestAndWaitForReceiptAsync(PermitFunction permitFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(permitFunction, cancellationToken);
        }

        public Task<string> PermitRequestAsync(string owner, string spender, BigInteger value, BigInteger deadline, byte v, byte[] r, byte[] s)
        {
            var permitFunction = new PermitFunction();
                permitFunction.Owner = owner;
                permitFunction.Spender = spender;
                permitFunction.Value = value;
                permitFunction.Deadline = deadline;
                permitFunction.V = v;
                permitFunction.R = r;
                permitFunction.S = s;
            
             return ContractHandler.SendRequestAsync(permitFunction);
        }

        public Task<TransactionReceipt> PermitRequestAndWaitForReceiptAsync(string owner, string spender, BigInteger value, BigInteger deadline, byte v, byte[] r, byte[] s, CancellationTokenSource cancellationToken = null)
        {
            var permitFunction = new PermitFunction();
                permitFunction.Owner = owner;
                permitFunction.Spender = spender;
                permitFunction.Value = value;
                permitFunction.Deadline = deadline;
                permitFunction.V = v;
                permitFunction.R = r;
                permitFunction.S = s;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(permitFunction, cancellationToken);
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

        public Task<string> SetBountyFeeRequestAsync(SetBountyFeeFunction setBountyFeeFunction)
        {
             return ContractHandler.SendRequestAsync(setBountyFeeFunction);
        }

        public Task<TransactionReceipt> SetBountyFeeRequestAndWaitForReceiptAsync(SetBountyFeeFunction setBountyFeeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setBountyFeeFunction, cancellationToken);
        }

        public Task<string> SetBountyFeeRequestAsync(BigInteger bountyFee)
        {
            var setBountyFeeFunction = new SetBountyFeeFunction();
                setBountyFeeFunction.BountyFee = bountyFee;
            
             return ContractHandler.SendRequestAsync(setBountyFeeFunction);
        }

        public Task<TransactionReceipt> SetBountyFeeRequestAndWaitForReceiptAsync(BigInteger bountyFee, CancellationTokenSource cancellationToken = null)
        {
            var setBountyFeeFunction = new SetBountyFeeFunction();
                setBountyFeeFunction.BountyFee = bountyFee;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setBountyFeeFunction, cancellationToken);
        }

        public Task<string> SetBountyWalletRequestAsync(SetBountyWalletFunction setBountyWalletFunction)
        {
             return ContractHandler.SendRequestAsync(setBountyWalletFunction);
        }

        public Task<TransactionReceipt> SetBountyWalletRequestAndWaitForReceiptAsync(SetBountyWalletFunction setBountyWalletFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setBountyWalletFunction, cancellationToken);
        }

        public Task<string> SetBountyWalletRequestAsync(string wallet)
        {
            var setBountyWalletFunction = new SetBountyWalletFunction();
                setBountyWalletFunction.Wallet = wallet;
            
             return ContractHandler.SendRequestAsync(setBountyWalletFunction);
        }

        public Task<TransactionReceipt> SetBountyWalletRequestAndWaitForReceiptAsync(string wallet, CancellationTokenSource cancellationToken = null)
        {
            var setBountyWalletFunction = new SetBountyWalletFunction();
                setBountyWalletFunction.Wallet = wallet;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setBountyWalletFunction, cancellationToken);
        }

        public Task<string> SetFeeManagerRequestAsync(SetFeeManagerFunction setFeeManagerFunction)
        {
             return ContractHandler.SendRequestAsync(setFeeManagerFunction);
        }

        public Task<TransactionReceipt> SetFeeManagerRequestAndWaitForReceiptAsync(SetFeeManagerFunction setFeeManagerFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setFeeManagerFunction, cancellationToken);
        }

        public Task<string> SetFeeManagerRequestAsync(string feeManager)
        {
            var setFeeManagerFunction = new SetFeeManagerFunction();
                setFeeManagerFunction.FeeManager = feeManager;
            
             return ContractHandler.SendRequestAsync(setFeeManagerFunction);
        }

        public Task<TransactionReceipt> SetFeeManagerRequestAndWaitForReceiptAsync(string feeManager, CancellationTokenSource cancellationToken = null)
        {
            var setFeeManagerFunction = new SetFeeManagerFunction();
                setFeeManagerFunction.FeeManager = feeManager;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setFeeManagerFunction, cancellationToken);
        }

        public Task<string> SetMaxTxPercentRequestAsync(SetMaxTxPercentFunction setMaxTxPercentFunction)
        {
             return ContractHandler.SendRequestAsync(setMaxTxPercentFunction);
        }

        public Task<TransactionReceipt> SetMaxTxPercentRequestAndWaitForReceiptAsync(SetMaxTxPercentFunction setMaxTxPercentFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setMaxTxPercentFunction, cancellationToken);
        }

        public Task<string> SetMaxTxPercentRequestAsync(BigInteger maxTxPercent)
        {
            var setMaxTxPercentFunction = new SetMaxTxPercentFunction();
                setMaxTxPercentFunction.MaxTxPercent = maxTxPercent;
            
             return ContractHandler.SendRequestAsync(setMaxTxPercentFunction);
        }

        public Task<TransactionReceipt> SetMaxTxPercentRequestAndWaitForReceiptAsync(BigInteger maxTxPercent, CancellationTokenSource cancellationToken = null)
        {
            var setMaxTxPercentFunction = new SetMaxTxPercentFunction();
                setMaxTxPercentFunction.MaxTxPercent = maxTxPercent;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setMaxTxPercentFunction, cancellationToken);
        }

        public Task<string> SetVentureFeeRequestAsync(SetVentureFeeFunction setVentureFeeFunction)
        {
             return ContractHandler.SendRequestAsync(setVentureFeeFunction);
        }

        public Task<TransactionReceipt> SetVentureFeeRequestAndWaitForReceiptAsync(SetVentureFeeFunction setVentureFeeFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setVentureFeeFunction, cancellationToken);
        }

        public Task<string> SetVentureFeeRequestAsync(BigInteger ventureFee)
        {
            var setVentureFeeFunction = new SetVentureFeeFunction();
                setVentureFeeFunction.VentureFee = ventureFee;
            
             return ContractHandler.SendRequestAsync(setVentureFeeFunction);
        }

        public Task<TransactionReceipt> SetVentureFeeRequestAndWaitForReceiptAsync(BigInteger ventureFee, CancellationTokenSource cancellationToken = null)
        {
            var setVentureFeeFunction = new SetVentureFeeFunction();
                setVentureFeeFunction.VentureFee = ventureFee;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setVentureFeeFunction, cancellationToken);
        }

        public Task<string> SetVentureWalletRequestAsync(SetVentureWalletFunction setVentureWalletFunction)
        {
             return ContractHandler.SendRequestAsync(setVentureWalletFunction);
        }

        public Task<TransactionReceipt> SetVentureWalletRequestAndWaitForReceiptAsync(SetVentureWalletFunction setVentureWalletFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setVentureWalletFunction, cancellationToken);
        }

        public Task<string> SetVentureWalletRequestAsync(string wallet)
        {
            var setVentureWalletFunction = new SetVentureWalletFunction();
                setVentureWalletFunction.Wallet = wallet;
            
             return ContractHandler.SendRequestAsync(setVentureWalletFunction);
        }

        public Task<TransactionReceipt> SetVentureWalletRequestAndWaitForReceiptAsync(string wallet, CancellationTokenSource cancellationToken = null)
        {
            var setVentureWalletFunction = new SetVentureWalletFunction();
                setVentureWalletFunction.Wallet = wallet;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setVentureWalletFunction, cancellationToken);
        }

        public Task<string> SymbolQueryAsync(SymbolFunction symbolFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SymbolFunction, string>(symbolFunction, blockParameter);
        }

        
        public Task<string> SymbolQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SymbolFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> TotalFeesQueryAsync(TotalFeesFunction totalFeesFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalFeesFunction, BigInteger>(totalFeesFunction, blockParameter);
        }

        
        public Task<BigInteger> TotalFeesQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalFeesFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> TotalSupplyQueryAsync(TotalSupplyFunction totalSupplyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(totalSupplyFunction, blockParameter);
        }

        
        public Task<BigInteger> TotalSupplyQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> TransferRequestAsync(TransferFunction transferFunction)
        {
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public Task<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(TransferFunction transferFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public Task<string> TransferRequestAsync(string recipient, BigInteger amount)
        {
            var transferFunction = new TransferFunction();
                transferFunction.Recipient = recipient;
                transferFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public Task<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(string recipient, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferFunction = new TransferFunction();
                transferFunction.Recipient = recipient;
                transferFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public Task<string> TransferFromRequestAsync(TransferFromFunction transferFromFunction)
        {
             return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public Task<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(TransferFromFunction transferFromFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }

        public Task<string> TransferFromRequestAsync(string sender, string recipient, BigInteger amount)
        {
            var transferFromFunction = new TransferFromFunction();
                transferFromFunction.Sender = sender;
                transferFromFunction.Recipient = recipient;
                transferFromFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public Task<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(string sender, string recipient, BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var transferFromFunction = new TransferFromFunction();
                transferFromFunction.Sender = sender;
                transferFromFunction.Recipient = recipient;
                transferFromFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
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

        public Task<string> UnbanRequestAsync(UnbanFunction unbanFunction)
        {
             return ContractHandler.SendRequestAsync(unbanFunction);
        }

        public Task<TransactionReceipt> UnbanRequestAndWaitForReceiptAsync(UnbanFunction unbanFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unbanFunction, cancellationToken);
        }

        public Task<string> UnbanRequestAsync(string wallet)
        {
            var unbanFunction = new UnbanFunction();
                unbanFunction.Wallet = wallet;
            
             return ContractHandler.SendRequestAsync(unbanFunction);
        }

        public Task<TransactionReceipt> UnbanRequestAndWaitForReceiptAsync(string wallet, CancellationTokenSource cancellationToken = null)
        {
            var unbanFunction = new UnbanFunction();
                unbanFunction.Wallet = wallet;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unbanFunction, cancellationToken);
        }

        public Task<string> UnpauseRequestAsync(UnpauseFunction unpauseFunction)
        {
             return ContractHandler.SendRequestAsync(unpauseFunction);
        }

        public Task<string> UnpauseRequestAsync()
        {
             return ContractHandler.SendRequestAsync<UnpauseFunction>();
        }

        public Task<TransactionReceipt> UnpauseRequestAndWaitForReceiptAsync(UnpauseFunction unpauseFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unpauseFunction, cancellationToken);
        }

        public Task<TransactionReceipt> UnpauseRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<UnpauseFunction>(null, cancellationToken);
        }

        public Task<string> VentureWalletQueryAsync(VentureWalletFunction ventureWalletFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<VentureWalletFunction, string>(ventureWalletFunction, blockParameter);
        }

        
        public Task<string> VentureWalletQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<VentureWalletFunction, string>(null, blockParameter);
        }
    }
}
