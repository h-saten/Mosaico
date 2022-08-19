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
using Mosaico.Integration.Blockchain.Ethereum.ContextUpgradeable.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.ContextUpgradeable
{
    public partial class ContextUpgradeableService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, ContextUpgradeableDeployment contextUpgradeableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<ContextUpgradeableDeployment>().SendRequestAndWaitForReceiptAsync(contextUpgradeableDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, ContextUpgradeableDeployment contextUpgradeableDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<ContextUpgradeableDeployment>().SendRequestAsync(contextUpgradeableDeployment);
        }

        public static async Task<ContextUpgradeableService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, ContextUpgradeableDeployment contextUpgradeableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, contextUpgradeableDeployment, cancellationTokenSource);
            return new ContextUpgradeableService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public ContextUpgradeableService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }


    }
}
