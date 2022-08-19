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
using Mosaico.Integration.Blockchain.Ethereum.MosaicoNativePaymaster.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.MosaicoNativePaymaster
{
    public partial class MosaicoNativePaymasterService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, MosaicoNativePaymasterDeployment mosaicoNativePaymasterDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<MosaicoNativePaymasterDeployment>().SendRequestAndWaitForReceiptAsync(mosaicoNativePaymasterDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, MosaicoNativePaymasterDeployment mosaicoNativePaymasterDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<MosaicoNativePaymasterDeployment>().SendRequestAsync(mosaicoNativePaymasterDeployment);
        }

        public static async Task<MosaicoNativePaymasterService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, MosaicoNativePaymasterDeployment mosaicoNativePaymasterDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, mosaicoNativePaymasterDeployment, cancellationTokenSource);
            return new MosaicoNativePaymasterService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public MosaicoNativePaymasterService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<BigInteger> CalldataSizeLimitQueryAsync(CalldataSizeLimitFunction calldataSizeLimitFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CalldataSizeLimitFunction, BigInteger>(calldataSizeLimitFunction, blockParameter);
        }

        
        public Task<BigInteger> CalldataSizeLimitQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<CalldataSizeLimitFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> ForwarderHubOverheadQueryAsync(ForwarderHubOverheadFunction forwarderHubOverheadFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ForwarderHubOverheadFunction, BigInteger>(forwarderHubOverheadFunction, blockParameter);
        }

        
        public Task<BigInteger> ForwarderHubOverheadQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<ForwarderHubOverheadFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> PaymasterAcceptanceBudgetQueryAsync(PaymasterAcceptanceBudgetFunction paymasterAcceptanceBudgetFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PaymasterAcceptanceBudgetFunction, BigInteger>(paymasterAcceptanceBudgetFunction, blockParameter);
        }

        
        public Task<BigInteger> PaymasterAcceptanceBudgetQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PaymasterAcceptanceBudgetFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> PostRelayedCallGasLimitQueryAsync(PostRelayedCallGasLimitFunction postRelayedCallGasLimitFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PostRelayedCallGasLimitFunction, BigInteger>(postRelayedCallGasLimitFunction, blockParameter);
        }

        
        public Task<BigInteger> PostRelayedCallGasLimitQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PostRelayedCallGasLimitFunction, BigInteger>(null, blockParameter);
        }

        public Task<BigInteger> PreRelayedCallGasLimitQueryAsync(PreRelayedCallGasLimitFunction preRelayedCallGasLimitFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PreRelayedCallGasLimitFunction, BigInteger>(preRelayedCallGasLimitFunction, blockParameter);
        }

        
        public Task<BigInteger> PreRelayedCallGasLimitQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<PreRelayedCallGasLimitFunction, BigInteger>(null, blockParameter);
        }

        public Task<GetGasAndDataLimitsOutputDTO> GetGasAndDataLimitsQueryAsync(GetGasAndDataLimitsFunction getGasAndDataLimitsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetGasAndDataLimitsFunction, GetGasAndDataLimitsOutputDTO>(getGasAndDataLimitsFunction, blockParameter);
        }

        public Task<GetGasAndDataLimitsOutputDTO> GetGasAndDataLimitsQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryDeserializingToObjectAsync<GetGasAndDataLimitsFunction, GetGasAndDataLimitsOutputDTO>(null, blockParameter);
        }

        public Task<string> GetHubAddrQueryAsync(GetHubAddrFunction getHubAddrFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetHubAddrFunction, string>(getHubAddrFunction, blockParameter);
        }

        
        public Task<string> GetHubAddrQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetHubAddrFunction, string>(null, blockParameter);
        }

        public Task<BigInteger> GetRelayHubDepositQueryAsync(GetRelayHubDepositFunction getRelayHubDepositFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetRelayHubDepositFunction, BigInteger>(getRelayHubDepositFunction, blockParameter);
        }

        
        public Task<BigInteger> GetRelayHubDepositQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GetRelayHubDepositFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }

        
        public Task<string> OwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(null, blockParameter);
        }

        public Task<string> PostRelayedCallRequestAsync(PostRelayedCallFunction postRelayedCallFunction)
        {
             return ContractHandler.SendRequestAsync(postRelayedCallFunction);
        }

        public Task<TransactionReceipt> PostRelayedCallRequestAndWaitForReceiptAsync(PostRelayedCallFunction postRelayedCallFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(postRelayedCallFunction, cancellationToken);
        }

        public Task<string> PostRelayedCallRequestAsync(byte[] context, bool success, BigInteger gasUseWithoutPost, RelayData relayData)
        {
            var postRelayedCallFunction = new PostRelayedCallFunction();
                postRelayedCallFunction.Context = context;
                postRelayedCallFunction.Success = success;
                postRelayedCallFunction.GasUseWithoutPost = gasUseWithoutPost;
                postRelayedCallFunction.RelayData = relayData;
            
             return ContractHandler.SendRequestAsync(postRelayedCallFunction);
        }

        public Task<TransactionReceipt> PostRelayedCallRequestAndWaitForReceiptAsync(byte[] context, bool success, BigInteger gasUseWithoutPost, RelayData relayData, CancellationTokenSource cancellationToken = null)
        {
            var postRelayedCallFunction = new PostRelayedCallFunction();
                postRelayedCallFunction.Context = context;
                postRelayedCallFunction.Success = success;
                postRelayedCallFunction.GasUseWithoutPost = gasUseWithoutPost;
                postRelayedCallFunction.RelayData = relayData;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(postRelayedCallFunction, cancellationToken);
        }

        public Task<string> PreRelayedCallRequestAsync(PreRelayedCallFunction preRelayedCallFunction)
        {
             return ContractHandler.SendRequestAsync(preRelayedCallFunction);
        }

        public Task<TransactionReceipt> PreRelayedCallRequestAndWaitForReceiptAsync(PreRelayedCallFunction preRelayedCallFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(preRelayedCallFunction, cancellationToken);
        }

        public Task<string> PreRelayedCallRequestAsync(RelayRequest relayRequest, byte[] signature, byte[] approvalData, BigInteger maxPossibleGas)
        {
            var preRelayedCallFunction = new PreRelayedCallFunction();
                preRelayedCallFunction.RelayRequest = relayRequest;
                preRelayedCallFunction.Signature = signature;
                preRelayedCallFunction.ApprovalData = approvalData;
                preRelayedCallFunction.MaxPossibleGas = maxPossibleGas;
            
             return ContractHandler.SendRequestAsync(preRelayedCallFunction);
        }

        public Task<TransactionReceipt> PreRelayedCallRequestAndWaitForReceiptAsync(RelayRequest relayRequest, byte[] signature, byte[] approvalData, BigInteger maxPossibleGas, CancellationTokenSource cancellationToken = null)
        {
            var preRelayedCallFunction = new PreRelayedCallFunction();
                preRelayedCallFunction.RelayRequest = relayRequest;
                preRelayedCallFunction.Signature = signature;
                preRelayedCallFunction.ApprovalData = approvalData;
                preRelayedCallFunction.MaxPossibleGas = maxPossibleGas;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(preRelayedCallFunction, cancellationToken);
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

        public Task<string> SetRelayHubRequestAsync(SetRelayHubFunction setRelayHubFunction)
        {
             return ContractHandler.SendRequestAsync(setRelayHubFunction);
        }

        public Task<TransactionReceipt> SetRelayHubRequestAndWaitForReceiptAsync(SetRelayHubFunction setRelayHubFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setRelayHubFunction, cancellationToken);
        }

        public Task<string> SetRelayHubRequestAsync(string hub)
        {
            var setRelayHubFunction = new SetRelayHubFunction();
                setRelayHubFunction.Hub = hub;
            
             return ContractHandler.SendRequestAsync(setRelayHubFunction);
        }

        public Task<TransactionReceipt> SetRelayHubRequestAndWaitForReceiptAsync(string hub, CancellationTokenSource cancellationToken = null)
        {
            var setRelayHubFunction = new SetRelayHubFunction();
                setRelayHubFunction.Hub = hub;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setRelayHubFunction, cancellationToken);
        }

        public Task<string> SetTargetRequestAsync(SetTargetFunction setTargetFunction)
        {
             return ContractHandler.SendRequestAsync(setTargetFunction);
        }

        public Task<TransactionReceipt> SetTargetRequestAndWaitForReceiptAsync(SetTargetFunction setTargetFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setTargetFunction, cancellationToken);
        }

        public Task<string> SetTargetRequestAsync(string target)
        {
            var setTargetFunction = new SetTargetFunction();
                setTargetFunction.Target = target;
            
             return ContractHandler.SendRequestAsync(setTargetFunction);
        }

        public Task<TransactionReceipt> SetTargetRequestAndWaitForReceiptAsync(string target, CancellationTokenSource cancellationToken = null)
        {
            var setTargetFunction = new SetTargetFunction();
                setTargetFunction.Target = target;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setTargetFunction, cancellationToken);
        }

        public Task<string> SetTrustedForwarderRequestAsync(SetTrustedForwarderFunction setTrustedForwarderFunction)
        {
             return ContractHandler.SendRequestAsync(setTrustedForwarderFunction);
        }

        public Task<TransactionReceipt> SetTrustedForwarderRequestAndWaitForReceiptAsync(SetTrustedForwarderFunction setTrustedForwarderFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setTrustedForwarderFunction, cancellationToken);
        }

        public Task<string> SetTrustedForwarderRequestAsync(string forwarder)
        {
            var setTrustedForwarderFunction = new SetTrustedForwarderFunction();
                setTrustedForwarderFunction.Forwarder = forwarder;
            
             return ContractHandler.SendRequestAsync(setTrustedForwarderFunction);
        }

        public Task<TransactionReceipt> SetTrustedForwarderRequestAndWaitForReceiptAsync(string forwarder, CancellationTokenSource cancellationToken = null)
        {
            var setTrustedForwarderFunction = new SetTrustedForwarderFunction();
                setTrustedForwarderFunction.Forwarder = forwarder;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(setTrustedForwarderFunction, cancellationToken);
        }

        public Task<bool> TargetsQueryAsync(TargetsFunction targetsFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TargetsFunction, bool>(targetsFunction, blockParameter);
        }

        
        public Task<bool> TargetsQueryAsync(string returnValue1, BlockParameter blockParameter = null)
        {
            var targetsFunction = new TargetsFunction();
                targetsFunction.ReturnValue1 = returnValue1;
            
            return ContractHandler.QueryAsync<TargetsFunction, bool>(targetsFunction, blockParameter);
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

        public Task<string> TrustedForwarderQueryAsync(TrustedForwarderFunction trustedForwarderFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TrustedForwarderFunction, string>(trustedForwarderFunction, blockParameter);
        }

        
        public Task<string> TrustedForwarderQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<TrustedForwarderFunction, string>(null, blockParameter);
        }

        public Task<string> UnsetTargetRequestAsync(UnsetTargetFunction unsetTargetFunction)
        {
             return ContractHandler.SendRequestAsync(unsetTargetFunction);
        }

        public Task<TransactionReceipt> UnsetTargetRequestAndWaitForReceiptAsync(UnsetTargetFunction unsetTargetFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unsetTargetFunction, cancellationToken);
        }

        public Task<string> UnsetTargetRequestAsync(string target)
        {
            var unsetTargetFunction = new UnsetTargetFunction();
                unsetTargetFunction.Target = target;
            
             return ContractHandler.SendRequestAsync(unsetTargetFunction);
        }

        public Task<TransactionReceipt> UnsetTargetRequestAndWaitForReceiptAsync(string target, CancellationTokenSource cancellationToken = null)
        {
            var unsetTargetFunction = new UnsetTargetFunction();
                unsetTargetFunction.Target = target;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unsetTargetFunction, cancellationToken);
        }

        public Task<string> VersionPaymasterQueryAsync(VersionPaymasterFunction versionPaymasterFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<VersionPaymasterFunction, string>(versionPaymasterFunction, blockParameter);
        }

        
        public Task<string> VersionPaymasterQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<VersionPaymasterFunction, string>(null, blockParameter);
        }

        public Task<string> WithdrawRelayHubDepositToRequestAsync(WithdrawRelayHubDepositToFunction withdrawRelayHubDepositToFunction)
        {
             return ContractHandler.SendRequestAsync(withdrawRelayHubDepositToFunction);
        }

        public Task<TransactionReceipt> WithdrawRelayHubDepositToRequestAndWaitForReceiptAsync(WithdrawRelayHubDepositToFunction withdrawRelayHubDepositToFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawRelayHubDepositToFunction, cancellationToken);
        }

        public Task<string> WithdrawRelayHubDepositToRequestAsync(BigInteger amount, string target)
        {
            var withdrawRelayHubDepositToFunction = new WithdrawRelayHubDepositToFunction();
                withdrawRelayHubDepositToFunction.Amount = amount;
                withdrawRelayHubDepositToFunction.Target = target;
            
             return ContractHandler.SendRequestAsync(withdrawRelayHubDepositToFunction);
        }

        public Task<TransactionReceipt> WithdrawRelayHubDepositToRequestAndWaitForReceiptAsync(BigInteger amount, string target, CancellationTokenSource cancellationToken = null)
        {
            var withdrawRelayHubDepositToFunction = new WithdrawRelayHubDepositToFunction();
                withdrawRelayHubDepositToFunction.Amount = amount;
                withdrawRelayHubDepositToFunction.Target = target;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(withdrawRelayHubDepositToFunction, cancellationToken);
        }
    }
}
