using System.Collections.Generic;
using Mosaico.Integration.Blockchain.Ethereum.Configuration;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public interface IEthereumClientFactory
    {
        IEthereumClient GetClient(string network);
        IEthereumServiceFactory GetServiceFactory(string network);
        IAdminAccountProvider<EthereumAdminAccount> GetAdminAccount(string network);
        EthereumNetworkConfiguration GetConfiguration(string network);
        List<EthereumNetworkConfiguration> GetAllConfigurations();
    }
}