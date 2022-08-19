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
using Mosaico.Integration.Blockchain.Ethereum.SignatureCheckerUpgradeable.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.SignatureCheckerUpgradeable
{
    public partial class SignatureCheckerUpgradeableService
    {
        public static Task<TransactionReceipt> DeployContractAndWaitForReceiptAsync(Nethereum.Web3.Web3 web3, SignatureCheckerUpgradeableDeployment signatureCheckerUpgradeableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            return web3.Eth.GetContractDeploymentHandler<SignatureCheckerUpgradeableDeployment>().SendRequestAndWaitForReceiptAsync(signatureCheckerUpgradeableDeployment, cancellationTokenSource);
        }

        public static Task<string> DeployContractAsync(Nethereum.Web3.Web3 web3, SignatureCheckerUpgradeableDeployment signatureCheckerUpgradeableDeployment)
        {
            return web3.Eth.GetContractDeploymentHandler<SignatureCheckerUpgradeableDeployment>().SendRequestAsync(signatureCheckerUpgradeableDeployment);
        }

        public static async Task<SignatureCheckerUpgradeableService> DeployContractAndGetServiceAsync(Nethereum.Web3.Web3 web3, SignatureCheckerUpgradeableDeployment signatureCheckerUpgradeableDeployment, CancellationTokenSource cancellationTokenSource = null)
        {
            var receipt = await DeployContractAndWaitForReceiptAsync(web3, signatureCheckerUpgradeableDeployment, cancellationTokenSource);
            return new SignatureCheckerUpgradeableService(web3, receipt.ContractAddress);
        }

        protected Nethereum.Web3.Web3 Web3{ get; }

        public ContractHandler ContractHandler { get; }

        public SignatureCheckerUpgradeableService(Nethereum.Web3.Web3 web3, string contractAddress)
        {
            Web3 = web3;
            ContractHandler = web3.Eth.GetContractHandler(contractAddress);
        }


    }
}
