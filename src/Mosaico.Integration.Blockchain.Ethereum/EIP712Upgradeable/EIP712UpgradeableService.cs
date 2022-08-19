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
using Mosaico.Integration.Blockchain.Ethereum.EIP712Upgradeable.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.EIP712Upgradeable
{
    public partial class EIP712UpgradeableService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, EIP712UpgradeableDeployment eIP712UpgradeableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<EIP712UpgradeableDeployment>().SendRequestAndWaitForReceiptAsync(eIP712UpgradeableDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, EIP712UpgradeableDeployment eIP712UpgradeableDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<EIP712UpgradeableDeployment>().SendRequestAsync(eIP712UpgradeableDeployment);
        }

        public static async Task<EIP712UpgradeableService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, EIP712UpgradeableDeployment eIP712UpgradeableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, eIP712UpgradeableDeployment, cancellationTokenSource);
            return new EIP712UpgradeableService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public EIP712UpgradeableService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }


    }
}
