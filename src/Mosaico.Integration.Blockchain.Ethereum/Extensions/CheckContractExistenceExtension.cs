using System.Threading.Tasks;
using Nethereum.Web3;

namespace Mosaico.Integration.Blockchain.Ethereum.Extensions
{
    public static class CheckContractExistenceExtension
    {
        public static async Task<bool> ContractExistAsync(this IWeb3 web3, string contractAddress)
        {
            var response = await web3.Eth.GetCode.SendRequestAsync(contractAddress);
            return response != "0x";
        }
    }
}