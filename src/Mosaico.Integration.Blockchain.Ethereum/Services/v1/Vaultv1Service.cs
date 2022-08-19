using System;
using System.Numerics;
using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.MosaicoVaultv1;
using Mosaico.Integration.Blockchain.Ethereum.MosaicoVaultv1.ContractDefinition;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.RPC.Accounts;
using Nethereum.Web3;

namespace Mosaico.Integration.Blockchain.Ethereum.Services.v1
{
    public struct VaultDepositResponse
    {
        public string TransactionHash { get; set; }
        public string Id { get; set; }
    }

    public struct VaultTransferResponse
    {
        public string TransactionHash { get; set; }
        public long Status { get; set; }
    }
    
    public class Vaultv1Service : IVaultv1Service
    {
        private readonly IEthereumClientFactory _ethereumClientFactory;

        public Vaultv1Service(IEthereumClientFactory ethereumClientFactory)
        {
            _ethereumClientFactory = ethereumClientFactory;
        }

        public async Task<string> DeployAsync(IAccount account, string network)
        {
            var client = _ethereumClientFactory.GetClient(network);
            account ??= await client.GetAdminAccountAsync();
            var response = await client.DeployContractAsync<MosaicoVaultv1Deployment>(account);
            return response;
        }

        public async Task<decimal> BalanceAsync(string network, string id, string vaultAddress)
        {
            var client = _ethereumClientFactory.GetClient(network);
            var account = await client.GetAdminAccountAsync();
            var serviceFactory = _ethereumClientFactory.GetServiceFactory(network);
            var service = await serviceFactory.GetServiceAsync<MosaicoVaultv1Service>(vaultAddress, account);
            var idNumber = BigInteger.Parse(id);
            var deposit = await service.GetVaultByIdQueryAsync(idNumber);
            return Web3.Convert.FromWei(deposit.ReturnValue1.Amount, 18);
        }

        public async Task<VaultDepositResponse> CreateDepositAsync(IAccount account, string network, Action<CreateVaultConfiguration> buildConfig = null)
        {
            var config = GetCreateDepositDefaultConfig();
            buildConfig?.Invoke(config);
            var client = _ethereumClientFactory.GetClient(network);
            account ??= await client.GetAdminAccountAsync();
            var serviceFactory = _ethereumClientFactory.GetServiceFactory(network);
            var service = await serviceFactory.GetServiceAsync<MosaicoVaultv1Service>(config.VaultAddress, account);
            var amount = Web3.Convert.ToWei(config.Amount, config.Decimals);
            var unlockAt = config.AvailableAt.ToUnixTimeSeconds();
            var depositFunction = new DepositFunction
            {
                Token = config.TokenAddress,
                Withdrawer = config.Recipient,
                Amount = amount,
                UnlockTimestamp = new BigInteger(unlockAt)
            };
            var receipt = await service.DepositRequestAndWaitForReceiptAsync(depositFunction);
            var count = await service.DepositsCountQueryAsync();
            return new VaultDepositResponse
            {
                Id = count.ToString(),
                TransactionHash = receipt.TransactionHash
            };
        }

        public async Task<VaultTransferResponse> SendAsync(IAccount account, string network, Action<VaultSendConfiguration> buildConfig)
        {
            var config = GetVaultSendDefaultConfig();
            buildConfig?.Invoke(config);
            var client = _ethereumClientFactory.GetClient(network);
            account ??= await client.GetAdminAccountAsync();
            var serviceFactory = _ethereumClientFactory.GetServiceFactory(network);
            var service = await serviceFactory.GetServiceAsync<MosaicoVaultv1Service>(config.VaultAddress, account);
            var amount = Web3.Convert.ToWei(config.Amount, 18);
            var receipt = await service.SendRequestAndWaitForReceiptAsync(BigInteger.Parse(config.Id), config.Recipient, amount);
            return new VaultTransferResponse
            {
                Status = receipt.Status.ToLong(),
                TransactionHash = receipt.TransactionHash
            };
        }

        private VaultSendConfiguration GetVaultSendDefaultConfig()
        {
            return new VaultSendConfiguration
            {
                Amount = 0
            };
        }

        private CreateVaultConfiguration GetCreateDepositDefaultConfig()
        {
            return new CreateVaultConfiguration
            {
                Amount = 0,
                AvailableAt = DateTimeOffset.UtcNow
            };
        }
    }
}