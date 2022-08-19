using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Configuration;

namespace Mosaico.Integration.Blockchain.Ethereum
{
    public class EthereumAdminAccountConfigurationProvider : IAdminAccountProvider<EthereumAdminAccount>
    {

        public EthereumNetworkConfiguration Configuration { get; set; }

        public Task<EthereumAdminAccount> GetAdminAccountDetailsAsync()
        {
            return Task.FromResult(Configuration.AdminAccount);
        }
    }
}