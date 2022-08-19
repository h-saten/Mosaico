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
using Mosaico.Integration.Blockchain.Ethereum.Ownable.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.Ownable
{
    public partial class OwnableService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, OwnableDeployment ownableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<OwnableDeployment>().SendRequestAndWaitForReceiptAsync(ownableDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, OwnableDeployment ownableDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<OwnableDeployment>().SendRequestAsync(ownableDeployment);
        }

        public static async Task<OwnableService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, OwnableDeployment ownableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, ownableDeployment, cancellationTokenSource);
            return new OwnableService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public OwnableService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<BigInteger> GeUnlockTimeQueryAsync(GeUnlockTimeFunction geUnlockTimeFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GeUnlockTimeFunction, BigInteger>(geUnlockTimeFunction, blockParameter);
        }

        
        public Task<BigInteger> GeUnlockTimeQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<GeUnlockTimeFunction, BigInteger>(null, blockParameter);
        }

        public Task<string> LockRequestAsync(LockFunction @lockFunction)
        {
             return ContractHandler.SendRequestAsync(@lockFunction);
        }

        public Task<TransactionReceipt> LockRequestAndWaitForReceiptAsync(LockFunction @lockFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(@lockFunction, cancellationToken);
        }

        public Task<string> LockRequestAsync(BigInteger time)
        {
            var @lockFunction = new LockFunction();
                @lockFunction.Time = time;
            
             return ContractHandler.SendRequestAsync(@lockFunction);
        }

        public Task<TransactionReceipt> LockRequestAndWaitForReceiptAsync(BigInteger time, CancellationTokenSource cancellationToken = null)
        {
            var @lockFunction = new LockFunction();
                @lockFunction.Time = time;
            
             return ContractHandler.SendRequestAndWaitForReceiptAsync(@lockFunction, cancellationToken);
        }

        public Task<string> OwnerQueryAsync(OwnerFunction ownerFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(ownerFunction, blockParameter);
        }

        
        public Task<string> OwnerQueryAsync(BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<OwnerFunction, string>(null, blockParameter);
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

        public Task<string> UnlockRequestAsync(UnlockFunction unlockFunction)
        {
             return ContractHandler.SendRequestAsync(unlockFunction);
        }

        public Task<string> UnlockRequestAsync()
        {
             return ContractHandler.SendRequestAsync<UnlockFunction>();
        }

        public Task<TransactionReceipt> UnlockRequestAndWaitForReceiptAsync(UnlockFunction unlockFunction, CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync(unlockFunction, cancellationToken);
        }

        public Task<TransactionReceipt> UnlockRequestAndWaitForReceiptAsync(CancellationTokenSource cancellationToken = null)
        {
             return ContractHandler.SendRequestAndWaitForReceiptAsync<UnlockFunction>(null, cancellationToken);
        }
    }
}
