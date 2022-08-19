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
using Mosaico.Integration.Blockchain.Ethereum.CountersUpgradeable.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.CountersUpgradeable
{
    public partial class CountersUpgradeableService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, CountersUpgradeableDeployment countersUpgradeableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<CountersUpgradeableDeployment>().SendRequestAndWaitForReceiptAsync(countersUpgradeableDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, CountersUpgradeableDeployment countersUpgradeableDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<CountersUpgradeableDeployment>().SendRequestAsync(countersUpgradeableDeployment);
        }

        public static async Task<CountersUpgradeableService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, CountersUpgradeableDeployment countersUpgradeableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, countersUpgradeableDeployment, cancellationTokenSource);
            return new CountersUpgradeableService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public CountersUpgradeableService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }


    }
}
