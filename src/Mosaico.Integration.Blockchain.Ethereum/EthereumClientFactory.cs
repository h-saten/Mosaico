using System.Collections.Generic;
using Autofac.Features.Indexed;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Configuration;
using Mosaico.Integration.Blockchain.Ethereum.Exceptions;

namespace Mosaico.Integration.Blockchain.Ethereum
{
    public class EthereumClientFactory : IEthereumClientFactory
    {
        private readonly IIndex<string, IEthereumClient> _ethereumClients;
        private readonly IIndex<string, IEthereumServiceFactory> _serviceFactories;
        private readonly IIndex<string, IAdminAccountProvider<EthereumAdminAccount>> _adminAccounts;
        private readonly IIndex<string, EthereumNetworkConfiguration> _configurations;

        public EthereumClientFactory(IIndex<string, IEthereumClient> ethereumClients, IIndex<string, IEthereumServiceFactory> serviceFactories, IIndex<string, IAdminAccountProvider<EthereumAdminAccount>> adminAccounts, IIndex<string, EthereumNetworkConfiguration> configurations)
        {
            _ethereumClients = ethereumClients;
            _serviceFactories = serviceFactories;
            _adminAccounts = adminAccounts;
            _configurations = configurations;
        }

        public IEthereumClient GetClient(string network)
        {
            if (_ethereumClients.TryGetValue(network, out var client))
            {
                client.AdminAccountProvider = GetAdminAccount(network);
                client.Configuration = GetConfiguration(network);
                return client;
            }

            throw new InvalidNetworkException(network);
        }

        public IEthereumServiceFactory GetServiceFactory(string network)
        {
            if (_serviceFactories.TryGetValue(network, out var factory))
            {
                factory.AdminAccountProvider = GetAdminAccount(network);
                factory.Configuration = GetConfiguration(network);
                return factory;
            }

            throw new InvalidNetworkException(network);
        }
        
        public IAdminAccountProvider<EthereumAdminAccount> GetAdminAccount(string network)
        {
            if (_adminAccounts.TryGetValue(network, out var factory))
            {
                factory.Configuration = GetConfiguration(network);
                return factory;
            }

            throw new InvalidNetworkException(network);
        }

        public EthereumNetworkConfiguration GetConfiguration(string network)
        {
            if (_configurations.TryGetValue(network, out var factory))
            {
                return factory;
            }

            throw new InvalidNetworkException(network);
        }

        public List<EthereumNetworkConfiguration> GetAllConfigurations()
        {
            var networks = new List<EthereumNetworkConfiguration>();
            foreach (var networkName in Mosaico.Blockchain.Base.Constants.BlockchainNetworks.All)
            {
                if (_configurations.TryGetValue(networkName, out var config))
                {   
                    networks.Add(config);
                }
            }

            return networks;
        }
    }
}