using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public interface IContractAnalyzer
    {
        Task<bool> ContainAllFunctionsAsync(string network, string contractAddress, string[] functionNames);
        Task<Dictionary<string, bool>> FunctionsExistsAsync(string network, string contractAddress, params string[] functionNames);
    }
}