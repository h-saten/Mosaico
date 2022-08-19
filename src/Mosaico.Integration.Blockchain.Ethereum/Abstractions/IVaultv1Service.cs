using System;
using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Services.v1;
using Nethereum.RPC.Accounts;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public interface IVaultv1Service
    {
        public Task<string> DeployAsync(IAccount account, string network);
        Task<VaultDepositResponse> CreateDepositAsync(IAccount account, string network, Action<CreateVaultConfiguration> buildConfig = null);
        Task<decimal> BalanceAsync(string network, string id, string vaultAddress);
        Task<VaultTransferResponse> SendAsync(IAccount account, string network, Action<VaultSendConfiguration> buildConfig);
    }
}