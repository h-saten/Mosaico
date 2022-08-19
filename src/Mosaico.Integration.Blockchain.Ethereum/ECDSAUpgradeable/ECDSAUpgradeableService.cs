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
using Mosaico.Integration.Blockchain.Ethereum.ECDSAUpgradeable.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.ECDSAUpgradeable
{
    public partial class ECDSAUpgradeableService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, ECDSAUpgradeableDeployment eCDSAUpgradeableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<ECDSAUpgradeableDeployment>().SendRequestAndWaitForReceiptAsync(eCDSAUpgradeableDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, ECDSAUpgradeableDeployment eCDSAUpgradeableDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<ECDSAUpgradeableDeployment>().SendRequestAsync(eCDSAUpgradeableDeployment);
        }

        public static async Task<ECDSAUpgradeableService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, ECDSAUpgradeableDeployment eCDSAUpgradeableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, eCDSAUpgradeableDeployment, cancellationTokenSource);
            return new ECDSAUpgradeableService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public ECDSAUpgradeableService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }


    }
}
