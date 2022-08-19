using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1;
using Mosaico.Integration.Blockchain.Ethereum.MosaicoERC20v1.ContractDefinition;

namespace Mosaico.Integration.Blockchain.Ethereum.Tests.Helpers
{
    public static class ERC20Deployer
    {
        public static string TokenName = "MOS T TOKEN";
        public static string TokenSymbol = "MOSTTO";
        public static long InitialSupply = 10000;
        
        public static async Task<string> DeployERC20(this IEthereumClient client, string tokenName = null, string tokenSymbol = null, long? initialSupply = null, string ownerAddress = null) => await client
            .DeployContractAsync<MosaicoERC20v1Deployment>(
                null, 
                MosaicoERC20v1Extensions.GetSettings(
                    tokenName ?? TokenName,
                    tokenSymbol ?? TokenSymbol, 
                    initialSupply ?? InitialSupply,
                    walletAddress: ownerAddress));
    }
}