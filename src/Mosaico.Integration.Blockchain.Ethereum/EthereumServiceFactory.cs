using System;
using System.Threading.Tasks;
using FluentValidation;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Configuration;
using Nethereum.RPC.Accounts;

namespace Mosaico.Integration.Blockchain.Ethereum
{
    public class EthereumServiceFactory : EthereumBase, IEthereumServiceFactory
    {
        public EthereumServiceFactory(IValidator<EthereumNetworkConfiguration> configValidator = null) : base(configValidator)
        {
        }

        public async Task<TService> GetServiceAsync<TService>(string contractAddress, IAccount account = null)
        {
            account ??= await GetAdminAccountAsync();
            var web3 = GetClient(account);
            return (TService)Activator.CreateInstance(typeof(TService), web3, contractAddress);
        }
        
        public async Task<TService> GetServiceAsync<TService>(string contractAddress, string privateKey = null)
        {
            IAccount account;
            if (string.IsNullOrWhiteSpace(privateKey))
            {
                account = await GetAdminAccountAsync();
            }
            else
            {
                account = await GetAccountAsync(privateKey);
            }
            
            var web3 = GetClient(account);
            return (TService)Activator.CreateInstance(typeof(TService), web3, contractAddress);
        }
    }
}