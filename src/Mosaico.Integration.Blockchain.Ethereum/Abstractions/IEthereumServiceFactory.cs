using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Configuration;
using Nethereum.RPC.Accounts;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public interface IEthereumServiceFactory
    {
        IAdminAccountProvider<EthereumAdminAccount> AdminAccountProvider { get; set; }
        EthereumNetworkConfiguration Configuration { get; set; }
        Task<TService> GetServiceAsync<TService>(string contractAddress, IAccount account = null);
        Task<TService> GetServiceAsync<TService>(string contractAddress, string privateKey = null);
    }
}