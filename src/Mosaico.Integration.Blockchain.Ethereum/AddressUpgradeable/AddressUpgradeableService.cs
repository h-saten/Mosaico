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
using Mosaico.Integration.Blockchain.Ethereum.AddressUpgradeable.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.AddressUpgradeable
{
    public partial class AddressUpgradeableService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, AddressUpgradeableDeployment addressUpgradeableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<AddressUpgradeableDeployment>().SendRequestAndWaitForReceiptAsync(addressUpgradeableDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, AddressUpgradeableDeployment addressUpgradeableDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<AddressUpgradeableDeployment>().SendRequestAsync(addressUpgradeableDeployment);
        }

        public static async Task<AddressUpgradeableService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, AddressUpgradeableDeployment addressUpgradeableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, addressUpgradeableDeployment, cancellationTokenSource);
            return new AddressUpgradeableService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public AddressUpgradeableService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }


    }
}
