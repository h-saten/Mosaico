using System;
using System.Numerics;
using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Models;
using Mosaico.Integration.Blockchain.Ethereum.Tether.TetherToken;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.RPC.Accounts;
using Nethereum.Web3;
using Serilog;

namespace Mosaico.Integration.Blockchain.Ethereum.PaymentTokens
{
    public class TetherPaymentTokenService : IPaymentTokenService
    {
        private readonly ILogger _logger;
        private readonly IEthereumClientFactory _ethereumClientFactory;

        public TetherPaymentTokenService(IEthereumClientFactory ethereumClientFactory, ILogger logger = null)
        {
            _ethereumClientFactory = ethereumClientFactory;
            _logger = logger;
        }
        
        public async Task<decimal> GetAllowanceAsync(string network, Action<AllowanceConfiguration> buildConfig = null)
        {
            var config = new AllowanceConfiguration();
            buildConfig?.Invoke(config);
            var client = _ethereumClientFactory.GetClient(network);
            var serviceClient = _ethereumClientFactory.GetServiceFactory(network);
            var ownerAccount = await client.GetAccountAsync(config.OwnerPrivateKey);
            var contract =  await serviceClient.GetServiceAsync<TetherTokenService>(config.ContractAddress, ownerAccount);
            var response = await contract.AllowanceQueryAsync(config.OwnerAddress, config.SpenderAddress);
            return Web3.Convert.FromWei(response);
        }

        public async Task SetWalletAllowanceAsync(string network, Action<AllowanceConfiguration> buildConfig = null)
        {
            var config = new AllowanceConfiguration();
            buildConfig?.Invoke(config);
            var client = _ethereumClientFactory.GetClient(network);
            var serviceClient = _ethereumClientFactory.GetServiceFactory(network);
            var ownerAccount = await client.GetAccountAsync(config.OwnerPrivateKey);
            var contract =
                await serviceClient.GetServiceAsync<TetherTokenService>(config.ContractAddress, ownerAccount);
            var weiAmount = Web3.Convert.ToWei(config.Amount, config.Decimals);

            _logger?.Information(
                $"[BLOCKCHAIN][ALLOWANCE] msgSender/owner: '{ownerAccount.Address}' | spender: {config.SpenderAddress} | amount: {weiAmount}");

            var allowanceAmount = await contract.AllowanceQueryAsync(ownerAccount.Address, config.SpenderAddress);
            if (allowanceAmount > 0 && weiAmount > 0)
            {
                var zeroAllowance = await contract.ApproveRequestAndWaitForReceiptAsync(config.SpenderAddress, 0);
                if (zeroAllowance.Status.ToLong() != 1)
                {
                    _logger?.Error(
                        $"Ethereum Transaction failed for contract {config.ContractAddress} - allowance function, resetting - set zero");
                    throw new EthereumTransactionFailedException(zeroAllowance.TransactionHash);
                }
            }
            
            var response = await contract.ApproveRequestAndWaitForReceiptAsync(config.SpenderAddress, weiAmount);
            if (response.Status.ToLong() != 1)
            {
                _logger?.Error(
                    $"Ethereum Transaction failed for contract {config.ContractAddress} - allowance function");
                throw new EthereumTransactionFailedException(response.TransactionHash);
            }
        }

        public async Task<decimal> BalanceOfAsync(string network, string contractAddress, string accountAddress)
        {
            var client = _ethereumClientFactory.GetClient(network);
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var admin = await client.GetAdminAccountAsync();
            var erc20Service = await service.GetServiceAsync<TetherTokenService>(contractAddress, admin);
            var decimalsAmount = 18;
            try
            {
                decimalsAmount = (int) await erc20Service.DecimalsQueryAsync();
            }
            catch (Exception ex)
            {
                _logger?.Warning($"Error occured during convertion of decimals to int. - {ex.Message} / {ex.StackTrace}");
            }
            var balanceResponse = await erc20Service.BalanceOfQueryAsync(accountAddress);
            var divisor = (decimal) new BigInteger(Math.Pow(10, decimalsAmount));
            return (decimal) balanceResponse / divisor;
        }
        
        public async Task<TransactionEstimate> EstimateTransferAsync(string network, string contractAddress, string recipientAddress)
        {
            var client = _ethereumClientFactory.GetClient(network);
            return await client.GetTransferEstimateAsync(null, contractAddress, recipientAddress);
        }

        public async Task<string> TransferAsync(string network, IAccount sender, string contractAddress, string recipient, decimal amount)
        {
            var service = _ethereumClientFactory.GetServiceFactory(network);
            var erc20Service = await service.GetServiceAsync<TetherTokenService>(contractAddress, sender);
            var decimalsAmount = 18;
            try
            {
                decimalsAmount = (int) await erc20Service.DecimalsQueryAsync();
            }
            catch (Exception ex)
            {
                _logger?.Warning($"Error occured during convertion of decimals to int. - {ex.Message} / {ex.StackTrace}");
            }

            var tokensToSend = Web3.Convert.ToWei(amount, decimalsAmount);
            var response = await erc20Service.TransferRequestAndWaitForReceiptAsync(recipient, tokensToSend);
            if (response.Status.Value == 0)
            {
                throw new EthereumTransactionFailedException($"Transaction on {contractAddress} has failed.");
            }

            return response.TransactionHash;
        }
    }
}