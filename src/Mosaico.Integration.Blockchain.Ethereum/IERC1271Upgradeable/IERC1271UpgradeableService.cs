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
using Mosaico.Integration.Blockchain.Ethereum.IERC1271Upgradeable.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.IERC1271Upgradeable
{
    public partial class IERC1271UpgradeableService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, IERC1271UpgradeableDeployment iERC1271UpgradeableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<IERC1271UpgradeableDeployment>().SendRequestAndWaitForReceiptAsync(iERC1271UpgradeableDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, IERC1271UpgradeableDeployment iERC1271UpgradeableDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<IERC1271UpgradeableDeployment>().SendRequestAsync(iERC1271UpgradeableDeployment);
        }

        public static async Task<IERC1271UpgradeableService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, IERC1271UpgradeableDeployment iERC1271UpgradeableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, iERC1271UpgradeableDeployment, cancellationTokenSource);
            return new IERC1271UpgradeableService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public IERC1271UpgradeableService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }

        public Task<byte[]> IsValidSignatureQueryAsync(IsValidSignatureFunction isValidSignatureFunction, BlockParameter blockParameter = null)
        {
            return ContractHandler.QueryAsync<IsValidSignatureFunction, byte[]>(isValidSignatureFunction, blockParameter);
        }

        
        public Task<byte[]> IsValidSignatureQueryAsync(byte[] hash, byte[] signature, BlockParameter blockParameter = null)
        {
            var isValidSignatureFunction = new IsValidSignatureFunction();
                isValidSignatureFunction.Hash = hash;
                isValidSignatureFunction.Signature = signature;
            
            return ContractHandler.QueryAsync<IsValidSignatureFunction, byte[]>(isValidSignatureFunction, blockParameter);
        }
    }
}
