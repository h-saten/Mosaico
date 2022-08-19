using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Mosaico.Blockchain.Base.Extensions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Tether.TetherToken.ContractDefinition;
using Serilog;

namespace Mosaico.Integration.Blockchain.Ethereum.DataSeed
{
    public interface ILocalPaymentTokensDeployer
    {
        Task<string> DeployTetherAsync(string network, decimal initialSupply, string privateKey);
    }

    class LocalPaymentTokensDeployer : ILocalPaymentTokensDeployer
    {
        private readonly ILogger _logger;
        private readonly IEthereumClientFactory _ethereumClientFactory;

        public LocalPaymentTokensDeployer(IEthereumClientFactory ethereumClientFactory, ILogger logger = null)
        {
            _ethereumClientFactory = ethereumClientFactory;
            _logger = logger;
        }
        
        public async Task<string> DeployTetherAsync(string network, decimal initialSupply, string privateKey)
        {
            var settings = new Dictionary<string, object>
            {
                {"_initialSupply", initialSupply.ConvertToBigInteger(6)},
                {"_name", "Tether"},
                {"_symbol", "USDT"},
                {"_decimals", new BigInteger(6)},
            };
            
            var client = _ethereumClientFactory.GetClient(network);
            var account = await client.GetAccountAsync(privateKey);
            return await client.DeployContractAsync<TetherTokenDeployment>(account, settings);
        }
    }
}