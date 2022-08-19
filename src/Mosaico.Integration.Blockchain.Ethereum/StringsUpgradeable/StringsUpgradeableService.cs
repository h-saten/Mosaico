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
using Mosaico.Integration.Blockchain.Ethereum.StringsUpgradeable.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.StringsUpgradeable
{
    public partial class StringsUpgradeableService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, StringsUpgradeableDeployment stringsUpgradeableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<StringsUpgradeableDeployment>().SendRequestAndWaitForReceiptAsync(stringsUpgradeableDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, StringsUpgradeableDeployment stringsUpgradeableDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<StringsUpgradeableDeployment>().SendRequestAsync(stringsUpgradeableDeployment);
        }

        public static async Task<StringsUpgradeableService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, StringsUpgradeableDeployment stringsUpgradeableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, stringsUpgradeableDeployment, cancellationTokenSource);
            return new StringsUpgradeableService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public StringsUpgradeableService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }


    }
}
