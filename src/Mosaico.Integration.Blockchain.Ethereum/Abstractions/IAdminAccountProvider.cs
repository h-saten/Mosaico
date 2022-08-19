using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Configuration;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public interface IAdminAccountProvider<T>
    {
        public EthereumNetworkConfiguration Configuration { get; set; }
        Task<T> GetAdminAccountDetailsAsync();
    }
}