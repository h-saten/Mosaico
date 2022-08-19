using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Tether.TetherToken.ContractDefinition;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace Mosaico.Integration.Blockchain.Ethereum.Tether.TetherToken
{
    public partial class TetherTokenService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Web3 web3, TetherTokenDeployment tetherTokenDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<TetherTokenDeployment>().SendRequestAndWaitForReceiptAsync(tetherTokenDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Web3 web3, TetherTokenDeployment tetherTokenDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<TetherTokenDeployment>().SendRequestAsync(tetherTokenDeployment);
        }

        public static async Task<TetherTokenService> DeployContractAndGetServiceAsync(Web3 web3, TetherTokenDeployment tetherTokenDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, tetherTokenDeployment, cancellationTokenSource);
            return new TetherTokenService(web3, receipt.ContractAddress);
        }

        protected Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public TetherTokenService(Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<string> NameQueryAsync(NameFunction nameFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(nameFunction, blockParameter);
        }

        
        public Task<string> NameQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<NameFunction, string>(null, blockParameter);
        }

        public Task<string> DeprecateRequestAsync(DeprecateFunction deprecateFunction)
        {
             return ContractHandler.SendRequestAsync(deprecateFunction);
        }

        public Task<TransactionReceipt> DeprecateRequestAndWaitForReceiptAsync(DeprecateFunction deprecateFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deprecateFunction, cancellationToken);
        }

        public Task<string> DeprecateRequestAsync(string upgradedAddress)
        {
            var deprecateFunction = new DeprecateFunction();
                deprecateFunction.UpgradedAddress = upgradedAddress;
            
             return ContractHandler.SendRequestAsync(deprecateFunction);
        }

        public Task<TransactionReceipt> DeprecateRequestAndWaitForReceiptAsync(string upgradedAddress, CancellationTokenSource cancellationToken = null)
        {
            var deprecateFunction = new DeprecateFunction();
                deprecateFunction.UpgradedAddress = upgradedAddress;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(deprecateFunction, cancellationToken);
        }

        public Task<string> ApproveRequestAsync(ApproveFunction approveFunction)
        {
             return ContractHandler.SendRequestAsync(approveFunction);
        }

        public Task<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(ApproveFunction approveFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public Task<string> ApproveRequestAsync(string spender, BigInteger value)
        {
            var approveFunction = new ApproveFunction();
                approveFunction.Spender = spender;
                approveFunction.Value = value;
            
             return ContractHandler.SendRequestAsync(approveFunction);
        }

        public Task<TransactionReceipt> ApproveRequestAndWaitForReceiptAsync(string spender, BigInteger value, CancellationTokenSource cancellationToken = null)
        {
            var approveFunction = new ApproveFunction();
                approveFunction.Spender = spender;
                approveFunction.Value = value;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(approveFunction, cancellationToken);
        }

        public Task<bool> DeprecatedQueryAsync(DeprecatedFunction deprecatedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DeprecatedFunction, bool>(deprecatedFunction, blockParameter);
        }

        
        public Task<bool> DeprecatedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DeprecatedFunction, bool>(null, blockParameter);
        }

        public Task<string> AddBlackListRequestAsync(AddBlackListFunction addBlackListFunction)
        {
             return ContractHandler.SendRequestAsync(addBlackListFunction);
        }

        public Task<TransactionReceipt> AddBlackListRequestAndWaitForReceiptAsync(AddBlackListFunction addBlackListFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addBlackListFunction, cancellationToken);
        }

        public Task<string> AddBlackListRequestAsync(string evilUser)
        {
            var addBlackListFunction = new AddBlackListFunction();
                addBlackListFunction.EvilUser = evilUser;
            
             return ContractHandler.SendRequestAsync(addBlackListFunction);
        }

        public Task<TransactionReceipt> AddBlackListRequestAndWaitForReceiptAsync(string evilUser, CancellationTokenSource cancellationToken = null)
        {
            var addBlackListFunction = new AddBlackListFunction();
                addBlackListFunction.EvilUser = evilUser;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(addBlackListFunction, cancellationToken);
        }

        public Task<BigInteger> TotalSupplyQueryAsync(TotalSupplyFunction totalSupplyFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(totalSupplyFunction, blockParameter);
        }

        
        public Task<BigInteger> TotalSupplyQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> TransferFromRequestAsync(TransferFromFunction transferFromFunction)
        {
             return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public Task<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(TransferFromFunction transferFromFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }

        public Task<string> TransferFromRequestAsync(string from, string to, BigInteger value)
        {
            var transferFromFunction = new TransferFromFunction();
                transferFromFunction.From = from;
                transferFromFunction.To = to;
                transferFromFunction.Value = value;
            
             return ContractHandler.SendRequestAsync(transferFromFunction);
        }

        public Task<TransactionReceipt> TransferFromRequestAndWaitForReceiptAsync(string from, string to, BigInteger value, CancellationTokenSource cancellationToken = null)
        {
            var transferFromFunction = new TransferFromFunction();
                transferFromFunction.From = from;
                transferFromFunction.To = to;
                transferFromFunction.Value = value;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFromFunction, cancellationToken);
        }

        public Task<string> UpgradedAddressQueryAsync(UpgradedAddressFunction upgradedAddressFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<UpgradedAddressFunction, string>(upgradedAddressFunction, blockParameter);
        }

        
        public Task<string> UpgradedAddressQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<UpgradedAddressFunction, string>(null, blockParameter);
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

        public Task<BigInteger> DecimalsQueryAsync(DecimalsFunction decimalsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, BigInteger>(decimalsFunction, blockParameter);
        }

        
        public Task<BigInteger> DecimalsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<DecimalsFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> MaximumFeeQueryAsync(MaximumFeeFunction maximumFeeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MaximumFeeFunction, BigInteger>(maximumFeeFunction, blockParameter);
        }

        
        public Task<BigInteger> MaximumFeeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MaximumFeeFunction, BigInteger>(null, blockParameter);
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

        public Task<bool> GetBlackListStatusQueryAsync(GetBlackListStatusFunction getBlackListStatusFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetBlackListStatusFunction, bool>(getBlackListStatusFunction, blockParameter);
        }

        
        public Task<bool> GetBlackListStatusQueryAsync(string maker, BlockParameter blockParameter = null)
        {
            var getBlackListStatusFunction = new GetBlackListStatusFunction();
                getBlackListStatusFunction.Maker = maker;
            
            return ContractHandler.QueryAsync<GetBlackListStatusFunction, bool>(getBlackListStatusFunction, blockParameter);
        }

        public Task<BigInteger> AllowedQueryAsync(AllowedFunction allowedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<AllowedFunction, BigInteger>(allowedFunction, blockParameter);
        }

        
        public Task<BigInteger> AllowedQueryAsync(string returnValue1, string returnValue2, BlockParameter blockParameter = null)
        {
            var allowedFunction = new AllowedFunction();
                allowedFunction.ReturnValue1 = returnValue1;
                allowedFunction.ReturnValue2 = returnValue2;
            
            return ContractHandler.QueryAsync<AllowedFunction, BigInteger>(allowedFunction, blockParameter);
        }

        public Task<bool> PausedQueryAsync(PausedFunction pausedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PausedFunction, bool>(pausedFunction, blockParameter);
        }

        
        public Task<bool> PausedQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PausedFunction, bool>(null, blockParameter);
        }

        public Task<BigInteger> BalanceOfQueryAsync(BalanceOfFunction balanceOfFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
        }

        
        public Task<BigInteger> BalanceOfQueryAsync(string who, BlockParameter blockParameter = null)
        {
            var balanceOfFunction = new BalanceOfFunction();
                balanceOfFunction.Who = who;
            
            return ContractHandler.QueryAsync<BalanceOfFunction, BigInteger>(balanceOfFunction, blockParameter);
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

        public Task<string> GetOwnerQueryAsync(GetOwnerFunction getOwnerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetOwnerFunction, string>(getOwnerFunction, blockParameter);
        }

        
        public Task<string> GetOwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetOwnerFunction, string>(null, blockParameter);
        }

        public Task<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }

        
        public Task<string> OwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(null, blockParameter);
        }

        public Task<string> SymbolQueryAsync(SymbolFunction symbolFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SymbolFunction, string>(symbolFunction, blockParameter);
        }

        
        public Task<string> SymbolQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<SymbolFunction, string>(null, blockParameter);
        }

        public Task<string> TransferRequestAsync(TransferFunction transferFunction)
        {
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public Task<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(TransferFunction transferFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public Task<string> TransferRequestAsync(string to, BigInteger value)
        {
            var transferFunction = new TransferFunction();
                transferFunction.To = to;
                transferFunction.Value = value;
            
             return ContractHandler.SendRequestAsync(transferFunction);
        }

        public Task<TransactionReceipt> TransferRequestAndWaitForReceiptAsync(string to, BigInteger value, CancellationTokenSource cancellationToken = null)
        {
            var transferFunction = new TransferFunction();
                transferFunction.To = to;
                transferFunction.Value = value;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(transferFunction, cancellationToken);
        }

        public Task<string> SetParamsRequestAsync(SetParamsFunction setParamsFunction)
        {
             return ContractHandler.SendRequestAsync(setParamsFunction);
        }

        public Task<TransactionReceipt> SetParamsRequestAndWaitForReceiptAsync(SetParamsFunction setParamsFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setParamsFunction, cancellationToken);
        }

        public Task<string> SetParamsRequestAsync(BigInteger newBasisPoints, BigInteger newMaxFee)
        {
            var setParamsFunction = new SetParamsFunction();
                setParamsFunction.NewBasisPoints = newBasisPoints;
                setParamsFunction.NewMaxFee = newMaxFee;
            
             return ContractHandler.SendRequestAsync(setParamsFunction);
        }

        public Task<TransactionReceipt> SetParamsRequestAndWaitForReceiptAsync(BigInteger newBasisPoints, BigInteger newMaxFee, CancellationTokenSource cancellationToken = null)
        {
            var setParamsFunction = new SetParamsFunction();
                setParamsFunction.NewBasisPoints = newBasisPoints;
                setParamsFunction.NewMaxFee = newMaxFee;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setParamsFunction, cancellationToken);
        }

        public Task<string> IssueRequestAsync(IssueFunction issueFunction)
        {
             return ContractHandler.SendRequestAsync(issueFunction);
        }

        public Task<TransactionReceipt> IssueRequestAndWaitForReceiptAsync(IssueFunction issueFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(issueFunction, cancellationToken);
        }

        public Task<string> IssueRequestAsync(BigInteger amount)
        {
            var issueFunction = new IssueFunction();
                issueFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(issueFunction);
        }

        public Task<TransactionReceipt> IssueRequestAndWaitForReceiptAsync(BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var issueFunction = new IssueFunction();
                issueFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(issueFunction, cancellationToken);
        }

        public Task<string> RedeemRequestAsync(RedeemFunction redeemFunction)
        {
             return ContractHandler.SendRequestAsync(redeemFunction);
        }

        public Task<TransactionReceipt> RedeemRequestAndWaitForReceiptAsync(RedeemFunction redeemFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(redeemFunction, cancellationToken);
        }

        public Task<string> RedeemRequestAsync(BigInteger amount)
        {
            var redeemFunction = new RedeemFunction();
                redeemFunction.Amount = amount;
            
             return ContractHandler.SendRequestAsync(redeemFunction);
        }

        public Task<TransactionReceipt> RedeemRequestAndWaitForReceiptAsync(BigInteger amount, CancellationTokenSource cancellationToken = null)
        {
            var redeemFunction = new RedeemFunction();
                redeemFunction.Amount = amount;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(redeemFunction, cancellationToken);
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

        public Task<BigInteger> BasisPointsRateQueryAsync(BasisPointsRateFunction basisPointsRateFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BasisPointsRateFunction, BigInteger>(basisPointsRateFunction, blockParameter);
        }

        
        public Task<BigInteger> BasisPointsRateQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<BasisPointsRateFunction, BigInteger>(null, blockParameter);
        }

        public Task<bool> IsBlackListedQueryAsync(IsBlackListedFunction isBlackListedFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsBlackListedFunction, bool>(isBlackListedFunction, blockParameter);
        }

        
        public Task<bool> IsBlackListedQueryAsync(string returnValue1, BlockParameter blockParameter = null)
        {
            var isBlackListedFunction = new IsBlackListedFunction();
                isBlackListedFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<IsBlackListedFunction, bool>(isBlackListedFunction, blockParameter);
        }

        public Task<string> RemoveBlackListRequestAsync(RemoveBlackListFunction removeBlackListFunction)
        {
             return ContractHandler.SendRequestAsync(removeBlackListFunction);
        }

        public Task<TransactionReceipt> RemoveBlackListRequestAndWaitForReceiptAsync(RemoveBlackListFunction removeBlackListFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(removeBlackListFunction, cancellationToken);
        }

        public Task<string> RemoveBlackListRequestAsync(string clearedUser)
        {
            var removeBlackListFunction = new RemoveBlackListFunction();
                removeBlackListFunction.ClearedUser = clearedUser;
            
             return ContractHandler.SendRequestAsync(removeBlackListFunction);
        }

        public Task<TransactionReceipt> RemoveBlackListRequestAndWaitForReceiptAsync(string clearedUser, CancellationTokenSource cancellationToken = null)
        {
            var removeBlackListFunction = new RemoveBlackListFunction();
                removeBlackListFunction.ClearedUser = clearedUser;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(removeBlackListFunction, cancellationToken);
        }

        public Task<BigInteger> MAX_UINTQueryAsync(MAX_UINTFunction mAX_UINTFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MAX_UINTFunction, BigInteger>(mAX_UINTFunction, blockParameter);
        }

        
        public Task<BigInteger> MAX_UINTQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<MAX_UINTFunction, BigInteger>(null, blockParameter);
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

        public Task<string> DestroyBlackFundsRequestAsync(DestroyBlackFundsFunction destroyBlackFundsFunction)
        {
             return ContractHandler.SendRequestAsync(destroyBlackFundsFunction);
        }

        public Task<TransactionReceipt> DestroyBlackFundsRequestAndWaitForReceiptAsync(DestroyBlackFundsFunction destroyBlackFundsFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(destroyBlackFundsFunction, cancellationToken);
        }

        public Task<string> DestroyBlackFundsRequestAsync(string blackListedUser)
        {
            var destroyBlackFundsFunction = new DestroyBlackFundsFunction();
                destroyBlackFundsFunction.BlackListedUser = blackListedUser;
            
             return ContractHandler.SendRequestAsync(destroyBlackFundsFunction);
        }

        public Task<TransactionReceipt> DestroyBlackFundsRequestAndWaitForReceiptAsync(string blackListedUser, CancellationTokenSource cancellationToken = null)
        {
            var destroyBlackFundsFunction = new DestroyBlackFundsFunction();
                destroyBlackFundsFunction.BlackListedUser = blackListedUser;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(destroyBlackFundsFunction, cancellationToken);
        }
    }
}
